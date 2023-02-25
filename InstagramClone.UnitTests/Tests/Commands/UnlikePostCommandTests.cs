using InstagramClone.Application.Queries.User;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using InstagramClone.UnitTests.Helpers;
using InstagramClone.UnitTests.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InstagramClone.UnitTests.Tests.Commands;

public class UnlikePostCommandTests : IClassFixture<InjectionFixture>
{
    private readonly IRepository<UserProfile> _userProfileRepository;
    private readonly IRepository<UserPost> _userPostRepository;
    private readonly IRepository<PostLike> _postLikeRepository;
    private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;

    public UnlikePostCommandTests(InjectionFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _userProfileRepository = serviceProvider.GetService<IRepository<UserProfile>>();
        _userPostRepository = serviceProvider.GetService<IRepository<UserPost>>();
        _postLikeRepository = serviceProvider.GetService<IRepository<PostLike>>();
        _authenticatedCurrentUserInfoProvider = serviceProvider.GetService<IAuthenticatedCurrentUserInfoProvider>();
    }

    [Fact]
    public async Task UnlikePostCommand_VerifyPostLikedSuccess()
    {
        var userInfo = _authenticatedCurrentUserInfoProvider.Get();
        var password = RandomHelper.GetRandomString(10);

        var testUser = await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, userInfo.UserName, password);

        var existingUserPost = _userPostRepository.Query.First();

        await _postLikeRepository.InsertAsync(new()
        {
            PostId = existingUserPost.Id,
            UserProfileId = testUser.Id,
        }, CancellationToken.None);
        await _postLikeRepository.SaveChangesAsync(CancellationToken.None);

        var unlikePostCommandHandler = new UnlikePostCommandHandler(
            _postLikeRepository, _authenticatedCurrentUserInfoProvider, _userProfileRepository);

        await unlikePostCommandHandler.Handle(new UnlikePostCommand(new() { PostId = existingUserPost.Id }), CancellationToken.None);

        var isLiked = await _postLikeRepository.Query
            .AnyAsync(pl => pl.UserProfileId == testUser.Id
                && pl.PostId == existingUserPost.Id, CancellationToken.None);

        Assert.False(isLiked);
    }
}