using AutoMapper;
using InstagramClone.Application.Helpers;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.User;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Commands.User
{
    public class RegisterUserCommand : IRequest<UserProfileDto>
    {
        public RegisterUserCommand(RegisterRequest request)
        {
            Request = request;
        }

        public RegisterRequest Request { get; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserProfileDto>
    {
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(ILogger<RegisterUserCommandHandler> logger,
            IRepository<UserProfile> userProfileRepository,
            IMapper mapper)
        {
            _logger = logger;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            _logger.LogInformation($"Register user: {request.UserName}");

            var userDto = _mapper.Map<UserProfileDto>(request);

            PasswordHashHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userProfile = _mapper.Map<UserProfile>(userDto);

            userProfile.PasswordHash = passwordHash;
            userProfile.PasswordSalt = passwordSalt;

            await _userProfileRepository.InsertAsync(userProfile, cancellationToken);
            await _userProfileRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserProfileDto>(userProfile);
        }
    }
}