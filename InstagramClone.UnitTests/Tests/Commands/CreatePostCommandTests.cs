using AutoMapper;
using InstagramClone.Application.Models.Post.Requests;
using InstagramClone.Application.Queries.User;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using InstagramClone.UnitTests.Helpers;
using InstagramClone.UnitTests.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InstagramClone.UnitTests.Tests.Commands;

public class CreatePostCommandTests : IClassFixture<InjectionFixture>
{
    private readonly IRepository<UserProfile> _userProfileRepository;
    private readonly IRepository<UserPost> _userPostRepository;
    private readonly IMapper _mapper;
    private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
    private readonly IMediator _mediator;

    public CreatePostCommandTests(InjectionFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _userProfileRepository = serviceProvider.GetService<IRepository<UserProfile>>();
        _userPostRepository = serviceProvider.GetService<IRepository<UserPost>>();
        _mapper = AutomapperProfileHelper.CreateMapper();
        _authenticatedCurrentUserInfoProvider = serviceProvider.GetService<IAuthenticatedCurrentUserInfoProvider>();
        _mediator = serviceProvider.GetService<IMediator>();
    }

    [Fact]
    public async Task CreatePostCommand_VerifyPostCreatedSuccess()
    {
        var userInfo = _authenticatedCurrentUserInfoProvider.Get();
        var password = RandomHelper.GetRandomString(10);

        var testUser = await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, userInfo.UserName, password);

        var createPostCommandHandler = new CreatePostCommandHandler(
            _userPostRepository, _authenticatedCurrentUserInfoProvider, _userProfileRepository, _mapper, _mediator);

        var createPostRequest = new CreatePostRequest()
        {
            Description = "Test Description"
        };

        var response = await createPostCommandHandler.Handle(new CreatePostCommand(createPostRequest), 
            CancellationToken.None);

        var userPost = await _userPostRepository.FindAsync(CancellationToken.None, response.Id);

        Assert.Equal(createPostRequest.Description, userPost.Description);
        Assert.Equal(testUser.Id, userPost.CreatedUserProfileId);
        Assert.Equal(DateTime.UtcNow.Date, userPost.PostCreatedAt.Date);
    }
}