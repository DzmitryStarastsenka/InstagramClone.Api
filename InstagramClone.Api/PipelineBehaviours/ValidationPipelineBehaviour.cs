﻿using FluentValidation;
using InstagramClone.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Api.PipelineBehaviours
{
    public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                throw new ValidationApiException(
                    failures.Select(x => new ValidatedField(
                       x.FormattedMessagePlaceholderValues["PropertyName"]?.ToString(),
                       Enum.TryParse(typeof(ValidationErrorCode), x.ErrorCode, out var error) ? (ValidationErrorCode)error : ValidationErrorCode.Invalid,
                       x.ErrorMessage))
                    );
            }

            return await next();
        }
    }
}