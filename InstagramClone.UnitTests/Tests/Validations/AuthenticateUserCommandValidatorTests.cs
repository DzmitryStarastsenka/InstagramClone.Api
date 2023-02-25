using FluentValidation;
using InstagramClone.Application.Commands.User;
using InstagramClone.Application.Validations.Users;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.UnitTests.Helpers;
using InstagramClone.UnitTests.Services;
using InstagramClone.UnitTests.Tests.Validations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InstagramClone.UnitTests.Tests.Validations;

public class AuthenticateUserCommandValidatorTests : ValidatorTestBase<AuthenticateUserCommand>, IClassFixture<InjectionFixture>
{
    private readonly IRepository<UserProfile> _userProfileRepository;

    public AuthenticateUserCommandValidatorTests(InjectionFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _userProfileRepository = serviceProvider.GetService<IRepository<UserProfile>>();
    }

    [Fact]
    public async Task AuthenticateUserCommandValidator_UserDoesNotExist()
    {
        var testUserName = $"testUserProfile{RandomHelper.GetRandomString(5)}@gmail.com";

        var validator = await CreateValidatorAsync();

        Action<AuthenticateUserCommand> mutation = c => c.Request.UserName = testUserName;

        var result = await ValidateAsync(mutation);
        var error = result.ShouldHaveValidationErrorFor(c => c.Request.UserName).First();
        Assert.Equal(UserValidationErrorMessages.UserNotFound, error.ErrorMessage);
    }

    [Fact]
    public async Task AuthenticateUserCommandValidator_InvalidEmaill()
    {
        var testUserName = $"testInvalidEmaill";

        var validator = await CreateValidatorAsync();

        Action<AuthenticateUserCommand> mutation = c => c.Request.UserName = testUserName;

        var result = await ValidateAsync(mutation);
        var error = result.ShouldHaveValidationErrorFor(c => c.Request.UserName).First();
        Assert.Equal(UserValidationErrorMessages.IncorrectEmail, error.ErrorMessage);
    }

    protected override async Task<AuthenticateUserCommand> CreateValidObjectAsync()
    {
        var testUserName = $"testUserProfile{RandomHelper.GetRandomString(5)}@gmail.com";
        var password = RandomHelper.GetRandomString(10);

        await CreateHelperService.AddNewTestClientUserAsync(_userProfileRepository, testUserName, password);

        var authenticateUserCommand = new AuthenticateUserCommand(new()
        {
            UserName = testUserName,
            Password = password
        });

        return authenticateUserCommand;
    }

    protected override async Task<IValidator<AuthenticateUserCommand>> CreateValidatorAsync()
    {
        return await Task.FromResult(new AuthenticateUserCommandValidator(_userProfileRepository));
    }
}