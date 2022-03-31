using AutoMapper;
using InstagramClone.Application.Models.User;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Queries.User
{
    public class GetUserInfoQuery : IRequest<UserProfileDto>
    {
        public GetUserInfoQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }

    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserProfileDto>
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;

        public GetUserInfoQueryHandler(IRepository<UserProfile> userProfileRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<UserProfileDto>(await _userProfileRepository.FindAsync(cancellationToken, request.UserId));
        }
    }
}