using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Domain.Jwt.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Commands.User
{
    public class ValidateAuthJwtTokenCommand : IRequest<Unit>
    {
        public ValidateAuthJwtTokenCommand(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }

    public class ValidateAuthJwtTokenCommandHandler : IRequestHandler<ValidateAuthJwtTokenCommand, Unit>
    {
        private readonly IUserJwtTokenGenerator _jwtTokenGenerator;

        public ValidateAuthJwtTokenCommandHandler(IUserJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Unit> Handle(ValidateAuthJwtTokenCommand command, CancellationToken cancellationToken)
        {
            // Validate token
            if (!_jwtTokenGenerator.ValidateJwtTokenWithoutLifetime(command.Token))
            {
                var validationResults = new List<ValidatedField>
                {
                    new ValidatedField(nameof(command.Token), ValidationErrorCode.Invalid, ValidateAuthJwtTokenCommandErrorMessages.InvalidToken)
                };

                throw new ValidationApiException(validationResults, ValidateAuthJwtTokenCommandErrorMessages.InvalidToken);
            }

            if (!_jwtTokenGenerator.ValidateLifeTime(command.Token))
            {
                var validationResults = new List<ValidatedField>
                {
                    new ValidatedField(nameof(command.Token), ValidationErrorCode.Expired, ValidateAuthJwtTokenCommandErrorMessages.TokenExpired)
                };

                throw new ValidationApiException(validationResults, ValidateAuthJwtTokenCommandErrorMessages.TokenExpired);
            }

            return await Task.FromResult(Unit.Value);
        }
    }
}