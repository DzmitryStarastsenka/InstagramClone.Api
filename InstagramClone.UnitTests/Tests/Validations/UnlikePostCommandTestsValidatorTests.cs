using FluentValidation;
using InstagramClone.Application.Queries.User;
using InstagramClone.Application.Validations.Posts.ErrorMessages;
using InstagramClone.Application.Validations.Users;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using InstagramClone.UnitTests.Helpers;
using InstagramClone.UnitTests.Services;
using InstagramClone.UnitTests.Tests.Validations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InstagramClone.UnitTests.Tests.Validations;

public class UnlikePostCommandTestsValidatorTests : ValidatorTestBase<UnlikePostCommand>, IClassFixture<InjectionFixture>
{
    private readonly IRepository<UserProfile> _userProfileRepository;
    private readonly IRepository<UserPost> _userPostRepository;
    private readonly IRepository<PostLike> _postLikeRepository;
    private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;

    public UnlikePostCommandTestsValidatorTests(InjectionFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _userProfileRepository = serviceProvider.GetService<IRepository<UserProfile>>();
        _userPostRepository = serviceProvider.GetService<IRepository<UserPost>>();
        _postLikeRepository = serviceProvider.GetService<IRepository<PostLike>>();
        _authenticatedCurrentUserInfoProvider = serviceProvider.GetService<IAuthenticatedCurrentUserInfoProvider>();
    }

    [Fact]
    public async Task UnlikePostCommandValidator_PostDoesNotExist()
    {
        var userInfo = _authenticatedCurrentUserInfoProvider.Get();
        var password = RandomHelper.GetRandomString(10);

        var testUser = await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, userInfo.UserName, password);

        var validator = await CreateValidatorAsync();

        Action<UnlikePostCommand> mutation = c => c.Request.PostId = int.MaxValue;

        var result = await ValidateAsync(mutation);
        var error = result.ShouldHaveValidationErrorFor(c => c.Request.PostId).First();
        Assert.Equal(PostValidationErrorMessages.PostNotFound, error.ErrorMessage);
    }

    [Fact]
    public async Task UnlikePostCommandValidator_NotLiked()
    {
        var userInfo = _authenticatedCurrentUserInfoProvider.Get();
        var password = RandomHelper.GetRandomString(10);

        var testUser = await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, userInfo.UserName, password);

        var validator = await CreateValidatorAsync();

        var likePostCommand = await CreateValidObjectAsync();
        Action<UnlikePostCommand> mutation = c => { };

        var result = await ValidateAsync(mutation);
        var error = result.ShouldHaveValidationErrorFor(c => c.Request.PostId).First();
        Assert.Equal(PostValidationErrorMessages.NotLiked, error.ErrorMessage);
    }

    protected override async Task<UnlikePostCommand> CreateValidObjectAsync()
    {
        var existingUserPost = _userPostRepository.Query.First();

        var unlikePostCommand = new UnlikePostCommand(new()
        {
            PostId = existingUserPost.Id
        });

        return await Task.FromResult(unlikePostCommand);
    }

    protected override async Task<IValidator<UnlikePostCommand>> CreateValidatorAsync()
    {
        return await Task.FromResult(new UnlikePostCommandValidator(
            _postLikeRepository, _userPostRepository,
            _authenticatedCurrentUserInfoProvider, _userProfileRepository));
    }
}