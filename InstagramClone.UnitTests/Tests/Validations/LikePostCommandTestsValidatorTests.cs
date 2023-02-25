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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InstagramClone.UnitTests.Tests.Validations;

public class LikePostCommandTestsValidatorTests : ValidatorTestBase<LikePostCommand>, IClassFixture<InjectionFixture>
{
    private readonly IRepository<UserProfile> _userProfileRepository;
    private readonly IRepository<UserPost> _userPostRepository;
    private readonly IRepository<PostLike> _postLikeRepository;
    private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;

    public LikePostCommandTestsValidatorTests(InjectionFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _userProfileRepository = serviceProvider.GetService<IRepository<UserProfile>>();
        _userPostRepository = serviceProvider.GetService<IRepository<UserPost>>();
        _postLikeRepository = serviceProvider.GetService<IRepository<PostLike>>();
        _authenticatedCurrentUserInfoProvider = serviceProvider.GetService<IAuthenticatedCurrentUserInfoProvider>();
    }

    [Fact]
    public async Task LikePostCommandValidator_PostDoesNotExist()
    {
        var userInfo = _authenticatedCurrentUserInfoProvider.Get();
        var password = RandomHelper.GetRandomString(10);

        var testUser = await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, userInfo.UserName, password);

        var validator = await CreateValidatorAsync();

        Action<LikePostCommand> mutation = c => c.Request.PostId = int.MaxValue;

        var result = await ValidateAsync(mutation);
        var error = result.ShouldHaveValidationErrorFor(c => c.Request.PostId).First();
        Assert.Equal(PostValidationErrorMessages.PostNotFound, error.ErrorMessage);
    }

    [Fact]
    public async Task LikePostCommandValidator_AlreadyLiked()
    {
        var userInfo = _authenticatedCurrentUserInfoProvider.Get();
        var password = RandomHelper.GetRandomString(10);

        var testUser = await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, userInfo.UserName, password);

        var validator = await CreateValidatorAsync();

        var likePostCommand = await CreateValidObjectAsync();
        Action<LikePostCommand> mutation = c => { };

        await _postLikeRepository.InsertAsync(new()
        {
            PostId = likePostCommand.Request.PostId,
            UserProfileId = testUser.Id
        }, CancellationToken.None);
        await _postLikeRepository.SaveChangesAsync(CancellationToken.None);

        var result = await ValidateAsync(mutation);
        var error = result.ShouldHaveValidationErrorFor(c => c.Request.PostId).First();
        Assert.Equal(PostValidationErrorMessages.AlreadyLiked, error.ErrorMessage);
    }

    protected override async Task<LikePostCommand> CreateValidObjectAsync()
    {
        var existingUserPost = _userPostRepository.Query.First();

        var likePostCommand = new LikePostCommand(new()
        {
            PostId = existingUserPost.Id
        });

        return await Task.FromResult(likePostCommand);
    }

    protected override async Task<IValidator<LikePostCommand>> CreateValidatorAsync()
    {
        return await Task.FromResult(new LikePostCommandValidator(
            _postLikeRepository, _userPostRepository,
            _authenticatedCurrentUserInfoProvider, _userProfileRepository));
    }
}