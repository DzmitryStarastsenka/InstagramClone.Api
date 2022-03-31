using FluentValidation;
using InstagramClone.Domain.Exceptions;

namespace InstagramClone.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithValidationErrorCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, ValidationErrorCode error)
        {
            return rule.WithErrorCode(error.ToString());
        }
    }
}