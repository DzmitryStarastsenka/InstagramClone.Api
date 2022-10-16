using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Commands.User
{
    public class SubscribeToUserCommand : IRequest<Unit>
    {
        public SubscribeToUserCommand(int publisherId)
        {
            PublisherId = publisherId;
        }

        public int PublisherId { get; }
    }

    public class SubscribeToUserCommandHandler : IRequestHandler<SubscribeToUserCommand, Unit>
    {
        private readonly IRepository<Subscription> _subscriptionRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<UserProfile> _userProfileRepository;

        public SubscribeToUserCommandHandler(IRepository<Subscription> subscriptionRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<UserProfile> userProfileRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<Unit> Handle(SubscribeToUserCommand command, CancellationToken cancellationToken)
        {
            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, cancellationToken);

            _subscriptionRepository.Insert(new Subscription
            {
                SubscriberId = currentUserId,
                PublisherId = command.PublisherId
            });

            await _subscriptionRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}