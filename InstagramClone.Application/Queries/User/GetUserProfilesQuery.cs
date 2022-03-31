using AutoMapper;
using InstagramClone.Application.Models.User;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Queries.User
{
    public class GetUserProfilesQuery : IRequest<List<UserProfileDto>>
    {
        public GetUserProfilesQuery(SieveModel request)
        {
            Request = request;
        }

        public SieveModel Request { get; }
    }

    public class GetUserProfilesQueryHandler : IRequestHandler<GetUserProfilesQuery, List<UserProfileDto>>
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;

        public GetUserProfilesQueryHandler(IRepository<UserProfile> userProfileRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<List<UserProfileDto>> Handle(GetUserProfilesQuery command, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<UserProfileDto>>(await _userProfileRepository.FilterUserProfiles(command.Request).ToListAsync(cancellationToken));
        }
    }
}