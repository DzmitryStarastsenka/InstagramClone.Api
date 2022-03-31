﻿using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Application.Validations.Posts.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Api.Filters
{
    public class UserAccessToUpdateCommentFilter : Attribute, IAsyncActionFilter
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<PostComment> _postCommentRepository;
        private readonly ILogger<UserAccessToUpdateCommentFilter> _logger;

        public UserAccessToUpdateCommentFilter(IRepository<UserProfile> userProfileRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<PostComment> postCommentRepository,
            ILogger<UserAccessToUpdateCommentFilter> logger)
        {
            _userProfileRepository = userProfileRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _postCommentRepository = postCommentRepository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, CancellationToken.None);

            if (context.ActionArguments.TryGetValue("id", out var commentId))
            {
                var createdUserProfileId = await _postCommentRepository.Query.AsNoTracking()
                    .Where(p => p.Id == Convert.ToInt32(commentId))
                    .Select(p => p.UserProfileId)
                    .FirstOrDefaultAsync(CancellationToken.None);

                if (createdUserProfileId <= 0 || createdUserProfileId == currentUserId)
                {
                    await next();
                }
                else
                {
                    _logger.LogDebug($"User {currentUserName} with UserId={currentUserId} doesn't have access to comment with id {commentId}");

                    context.HttpContext.Response.ContentType = "application/json";
                    context.Result = new ObjectResult(PostValidationErrorMessages.DontHaveAccessToComment)
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                }
            }
        }
    }
}