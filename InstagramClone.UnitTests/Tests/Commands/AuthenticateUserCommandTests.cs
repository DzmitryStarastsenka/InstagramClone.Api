using AutoMapper;
using InstagramClone.Application.Commands.User;
using InstagramClone.Application.Helpers;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Domain.Jwt.Interfaces;
using InstagramClone.UnitTests.Helpers;
using InstagramClone.UnitTests.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InstagramClone.UnitTests.Tests.Commands;

public class AuthenticateUserCommandTests : IClassFixture<InjectionFixture>
{
    private readonly IRepository<UserProfile> _userProfileRepository;
    private readonly ILogger<AuthenticateUserCommandHandler> _authenticateUserCommandHandlerLogger;
    private readonly IUserJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public AuthenticateUserCommandTests(InjectionFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _userProfileRepository = serviceProvider.GetService<IRepository<UserProfile>>();
        _authenticateUserCommandHandlerLogger = serviceProvider.GetService<ILogger<AuthenticateUserCommandHandler>>();
        _jwtTokenGenerator = serviceProvider.GetService<IUserJwtTokenGenerator>();
        _mapper = AutomapperProfileHelper.CreateMapper();
    }

    [Fact]
    public void AuthenticateUserCommand_VerifyGenerateUserPasswordSuccess()
    {
        var password = RandomHelper.GetRandomString(10);
        PasswordHashHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        Assert.True(PasswordHashHelper.VerifyPasswordHash(password, passwordHash, passwordSalt));
    }

    [Fact]
    public async Task AuthenticateUserCommand_Success()
    {
        var testUserName = $"testUserProfile{RandomHelper.GetRandomString(5)}@gmail.com";
        var password = RandomHelper.GetRandomString(10);

        var testUser = await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, testUserName, password);

        var authenticateUserCommandHandler = new AuthenticateUserCommandHandler(
            _authenticateUserCommandHandlerLogger, _userProfileRepository, _jwtTokenGenerator, _mapper);

        var response = await authenticateUserCommandHandler.Handle(new AuthenticateUserCommand(new()
        {
            UserName = testUserName,
            Password = password
        }), CancellationToken.None);

        Assert.Equal(testUser.UserName, response.UserProfile.UserName);
        Assert.True(_jwtTokenGenerator.ValidateToken(response.Token));
    }

    [Fact]
    public async Task AuthenticateUserCommand_IncorrectPassword()
    {
        var testUserName = $"testUserProfile{RandomHelper.GetRandomString(5)}@gmail.com";
        var password = RandomHelper.GetRandomString(10);

        var testUser = await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, testUserName, password);

        var authenticateUserCommandHandler = new AuthenticateUserCommandHandler(
            _authenticateUserCommandHandlerLogger, _userProfileRepository, _jwtTokenGenerator, _mapper);

        var exception = await Should.ThrowAsync<ValidationApiException>(async () =>
            await authenticateUserCommandHandler.Handle(new AuthenticateUserCommand(new()
            {
                UserName = testUserName,
                Password = password + RandomHelper.GetRandomString(10)
            }), CancellationToken.None));

        exception.ShouldNotBeNull();
        Assert.Equal(UserValidationErrorMessages.IncorrectPassword, exception.Message);
    }
}