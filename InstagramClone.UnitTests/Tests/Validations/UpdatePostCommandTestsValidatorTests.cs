using FluentValidation;
using InstagramClone.Application.Commands.User;
using InstagramClone.Application.Queries.User;
using InstagramClone.Application.Validations.Posts.ErrorMessages;
using InstagramClone.Application.Validations.Users;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
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

public class UpdatePostCommandTestsValidatorTests : ValidatorTestBase<UpdatePostCommand>, IClassFixture<InjectionFixture>
{
    private readonly IRepository<UserPost> _userPostRepository;

    public UpdatePostCommandTestsValidatorTests(InjectionFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _userPostRepository = serviceProvider.GetService<IRepository<UserPost>>();
    }

    [Fact]
    public async Task UpdatePostCommandValidator_PostDoesNotExist()
    {
        var validator = await CreateValidatorAsync();

        Action<UpdatePostCommand> mutation = c => c.Request.PostId = int.MaxValue;

        var result = await ValidateAsync(mutation);
        var error = result.ShouldHaveValidationErrorFor(c => c.Request.PostId).First();
        Assert.Equal(PostValidationErrorMessages.PostNotFound, error.ErrorMessage);
    }

    protected override async Task<UpdatePostCommand> CreateValidObjectAsync()
    {
        var existingUserPost = _userPostRepository.Query.First();

        var updatePostCommand = new UpdatePostCommand(new()
        {
            PostId = existingUserPost.Id,
            Description = existingUserPost.Description + "Test Description"
        });

        return await Task.FromResult(updatePostCommand);
    }

    protected override async Task<IValidator<UpdatePostCommand>> CreateValidatorAsync()
    {
        return await Task.FromResult(new UpdatePostCommandValidator(_userPostRepository));
    }
}