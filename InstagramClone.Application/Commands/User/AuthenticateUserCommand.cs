using AutoMapper;
using InstagramClone.Application.Helpers;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.User;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Domain.Jwt.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Commands.User
{
    public class AuthenticateUserCommand : IRequest<LoginResponse>
    {
        public AuthenticateUserCommand(LoginRequest request)
        {
            Request = request;
        }

        public LoginRequest Request { get; }
    }

    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, LoginResponse>
    {
        private readonly ILogger<AuthenticateUserCommandHandler> _logger;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IUserJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthenticateUserCommandHandler(ILogger<AuthenticateUserCommandHandler> logger,
            IRepository<UserProfile> userProfileRepository,
            IUserJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _logger = logger;
            _userProfileRepository = userProfileRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<LoginResponse> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            _logger.LogInformation($"Authenticate user: {request.UserName}");

            var userProfile = await _userProfileRepository.GetByUserNameAsync(request.UserName, cancellationToken);

            if (!PasswordHashHelper.VerifyPasswordHash(request.Password, userProfile.PasswordHash, userProfile.PasswordSalt))
            {
                var validationResults = new List<ValidatedField>
                {
                    new ValidatedField(nameof( command.Request.Password ), ValidationErrorCode.Invalid, UserValidationErrorMessages.IncorrectPassword),
                };

                var errorMessage = UserValidationErrorMessages.IncorrectPassword;

                _logger.LogDebug(errorMessage);

                throw new ValidationApiException(validationResults, errorMessage);

            }

            return new LoginResponse
            {
                Token = _jwtTokenGenerator.GenerateToken(userProfile.UserName, userProfile.FirstName + " " + userProfile.LastName),
                UserProfile = _mapper.Map<UserProfileDto>(userProfile),
            };
        }
    }
}