using EntityFrameworkCoreMock;
using InstagramClone.Application.Helpers;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.Jwt.Impl;
using InstagramClone.Domain.Jwt.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Domain.UserProviders;
using InstagramClone.Infrastructure.DAL.Context;
using InstagramClone.UnitTests.Helpers;
using InstagramClone.UnitTests.Repositories;
using InstagramClone.UnitTests.Services.User.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace InstagramClone.UnitTests;

public class InjectionFixture
{
    internal static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    public InjectionFixture()
    {
        var services = new ServiceCollection();

        var dbContextMock = new DbContextMock<InstagramCloneDbContext>(DummyOptions);
        var sieveProcessor = new Mock<ISieveProcessor>().Object;

        var moqUserProfileFakeRepo = new Mock<FakeRepository<UserProfile>>();

        #region Test user profiles data

        PasswordHashHelper.CreatePasswordHash(RandomHelper.GetRandomString(10), out byte[] passwordHash1, out byte[] passwordSalt1);
        PasswordHashHelper.CreatePasswordHash(RandomHelper.GetRandomString(10), out byte[] passwordHash2, out byte[] passwordSalt2);
        PasswordHashHelper.CreatePasswordHash(RandomHelper.GetRandomString(10), out byte[] passwordHash3, out byte[] passwordSalt3);

        var userProfiles = new List<UserProfile>()
        {
            new()
            {
                FirstName = "Test1",
                LastName = "User1",
                UserName = "testClient1@gmail.com",
                PasswordHash = passwordHash1,
                PasswordSalt = passwordSalt1,
                Role = UserRole.Client,
            },
            new()
            {
                FirstName = "Test2",
                LastName = "User2",
                UserName = "testClient2@gmail.com",
                PasswordHash = passwordHash2,
                PasswordSalt = passwordSalt2,
                Role = UserRole.Client,
            },
            new()
            {
                FirstName = "Test3",
                LastName = "User3",
                UserName = "testAdmin@gmail.com",
                PasswordHash = passwordHash3,
                PasswordSalt = passwordSalt3,
                Role = UserRole.Admin,
            }
        };

        #endregion

        moqUserProfileFakeRepo.Setup(p => p.SieveProcessor).Returns(sieveProcessor);
        moqUserProfileFakeRepo.Setup(p => p.Context).Returns(dbContextMock.Object);
        moqUserProfileFakeRepo.Setup(p => p.Set).Returns(dbContextMock.CreateDbSetMock(x => x.UserProfiles, 
            userProfiles).Object);

        services.AddScoped<IRepository<UserProfile>>(sp => moqUserProfileFakeRepo.Object);

        var moqUserPostFakeRepo = new Mock<FakeRepository<UserPost>>();

        #region Test user posts data

        var userPosts = new List<UserPost>()
        {
            new()
            {
                Description = "Test description 1",
                CreatedUserProfileId = 1,
                PostCreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new()
            {
                Description = "Test description 2",
                CreatedUserProfileId = 1,
                PostCreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new()
            {
                Description = "Test description 3",
                CreatedUserProfileId = 2,
                PostCreatedAt = DateTime.UtcNow.AddDays(-30)
            }
        };

        #endregion

        moqUserPostFakeRepo.Setup(p => p.SieveProcessor).Returns(sieveProcessor);
        moqUserPostFakeRepo.Setup(p => p.Context).Returns(dbContextMock.Object);
        moqUserPostFakeRepo.Setup(p => p.Set).Returns(dbContextMock.CreateDbSetMock(x => x.Posts, userPosts).Object);

        services.AddScoped<IRepository<UserPost>>(sp => moqUserPostFakeRepo.Object);

        var moqPostCommentFakeRepo = new Mock<FakeRepository<PostComment>>();

        #region Test post comments data

        var postComments = new List<PostComment>()
        {
            new()
            {
                PostId = 1,
                CommentCreatedAt = DateTime.UtcNow.AddDays(-29),
                UserProfileId = 3
            },
            new()
            {
                PostId = 1,
                CommentCreatedAt = DateTime.UtcNow.AddDays(-29),
                UserProfileId = 2
            },
            new()
            {
                PostId = 2,
                CommentCreatedAt = DateTime.UtcNow.AddDays(-29),
                UserProfileId = 1
            }
        };

        #endregion

        moqPostCommentFakeRepo.Setup(p => p.SieveProcessor).Returns(sieveProcessor);
        moqPostCommentFakeRepo.Setup(p => p.Context).Returns(dbContextMock.Object);
        moqPostCommentFakeRepo.Setup(p => p.Set).Returns(dbContextMock.CreateDbSetMock(x => x.Comments, postComments).Object);

        services.AddScoped<IRepository<PostComment>>(sp => moqPostCommentFakeRepo.Object);

        var moqPostLikeFakeRepo = new Mock<FakeRepository<PostLike>>();

        #region Test post likes data

        var postLikes = new List<PostLike>()
        {
            new()
            {
                PostId = 1,
                UserProfileId = 1
            },
            new()
            {
                PostId = 1,
                UserProfileId = 2
            },
            new()
            {
                PostId = 2,
                UserProfileId = 3
            }
        };

        #endregion

        moqPostLikeFakeRepo.Setup(p => p.SieveProcessor).Returns(sieveProcessor);
        moqPostLikeFakeRepo.Setup(p => p.Context).Returns(dbContextMock.Object);
        moqPostLikeFakeRepo.Setup(p => p.Set).Returns(dbContextMock.CreateDbSetMock(x => x.Likes,
            (e, _) => new { e.PostId, e.UserProfileId }, postLikes).Object);

        services.AddScoped<IRepository<PostLike>>(sp => moqPostLikeFakeRepo.Object);

        var moqSubscriptionFakeRepo = new Mock<FakeRepository<Subscription>>();

        moqSubscriptionFakeRepo.Setup(p => p.SieveProcessor).Returns(sieveProcessor);
        moqSubscriptionFakeRepo.Setup(p => p.Context).Returns(dbContextMock.Object);
        moqSubscriptionFakeRepo.Setup(p => p.Set).Returns(dbContextMock.CreateDbSetMock(x => x.Subscriptions,
            (e, _) => new { e.SubscriberId, e.PublisherId }, new List<Subscription>()).Object);

        services.AddScoped<IRepository<Subscription>>(sp => moqSubscriptionFakeRepo.Object);

        services.AddTransient<IAuthenticatedCurrentUserInfoProvider, AuthenticatedFakeUserInfoProvider>();

        services.AddLogging();
        services.AddScoped(sp => new Mock<ISieveProcessor>().Object);
        services.AddScoped(sp => new Mock<IMediator>().Object);

        services.AddScoped<IUserJwtTokenGenerator, AuthJwtTokenGenerator>();

        services.AddSingleton(Configuration);

        ServiceProvider = services.BuildServiceProvider();
    }

    public DbContextOptions<InstagramCloneDbContext> DummyOptions { get; } = new DbContextOptionsBuilder<InstagramCloneDbContext>().Options;

    public ServiceProvider ServiceProvider { get; set; }
}