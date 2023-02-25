using FluentValidation;
using FluentValidation.TestHelper;
using System;
using System.Threading.Tasks;

namespace InstagramClone.UnitTests.Tests.Validations;

public abstract class ValidatorTestBase<TModel>
{
    protected abstract Task<TModel> CreateValidObjectAsync();

    protected async Task<TestValidationResult<TModel>> ValidateAsync(Action<TModel> mutate)
    {
        var model = await CreateValidObjectAsync();
        mutate(model);

        var validator = await CreateValidatorAsync();

        return await validator.TestValidateAsync(model);
    }

    protected abstract Task<IValidator<TModel>> CreateValidatorAsync();
}
