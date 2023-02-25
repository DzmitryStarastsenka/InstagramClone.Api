using AutoMapper;
using InstagramClone.Application.Models.Post.Requests;
using InstagramClone.Application.Queries.User;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.UnitTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InstagramClone.UnitTests.Tests.Commands;

public class UpdatePostCommandTests : IClassFixture<InjectionFixture>
{
    private readonly IRepository<UserPost> _userPostRepository;
    private readonly IMapper _mapper;

    public UpdatePostCommandTests(InjectionFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _userPostRepository = serviceProvider.GetService<IRepository<UserPost>>();
        _mapper = AutomapperProfileHelper.CreateMapper();
    }

    [Fact]
    public async Task UpdatePostCommand_VerifyPostUpdatedSuccess()
    {
        var existingUserPost = _userPostRepository.Query.First();

        var updatePostCommandHandler = new UpdatePostCommandHandler(
            _userPostRepository, _mapper);

        var updatePostRequest = new UpdatePostRequest()
        {
            PostId= existingUserPost.Id,
            Description = existingUserPost.Description + "Test Description"
        };

        await updatePostCommandHandler.Handle(new UpdatePostCommand(updatePostRequest), 
            CancellationToken.None);

        var updatedUserPost = await _userPostRepository.FindAsync(CancellationToken.None, existingUserPost.Id);

        Assert.Equal(updatePostRequest.Description, updatedUserPost.Description);
        Assert.Equal(existingUserPost.CreatedUserProfileId, updatedUserPost.CreatedUserProfileId);
        Assert.Equal(DateTime.UtcNow.Date, updatedUserPost.PostEditedAt.Value.Date);
    }
}