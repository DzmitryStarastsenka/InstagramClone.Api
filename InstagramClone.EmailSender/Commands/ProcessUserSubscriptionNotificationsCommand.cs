using AutoMapper;
using InstagramClone.Application.Models;
using InstagramClone.Application.Models.User;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.EmailSender.Helpers;
using InstagramClone.EmailSender.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.EmailSender.Commands
{
    internal class ProcessUserSubscriptionNotificationsCommand : IRequest<Unit>
    {
        public ProcessUserSubscriptionNotificationsCommand(SubscribeNotification subscribeNotification)
        {
            SubscribeNotification = subscribeNotification;
        }

        public SubscribeNotification SubscribeNotification { get; }
    }

    internal class ProcessUserSubscriptionNotificationsCommandHandler : IRequestHandler<ProcessUserSubscriptionNotificationsCommand, Unit>
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IRepository<UserPost> _userPostRepository;
        private readonly EmailSenderHelper _emailSenderhelper;
        private readonly IMapper _mapper;

        public ProcessUserSubscriptionNotificationsCommandHandler(
            IRepository<UserProfile> userProfileRepository,
            IRepository<UserPost> userPostRepository,
            EmailSenderHelper emailSenderhelper,
            IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _userPostRepository = userPostRepository;
            _emailSenderhelper = emailSenderhelper;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(ProcessUserSubscriptionNotificationsCommand command, CancellationToken cancellationToken)
        {
            var subscribeNotification = command.SubscribeNotification;

            var subscribers = await _userProfileRepository.Query
                .Where(u => u.Id == subscribeNotification.CreatedUserProfileId)
                .SelectMany(u => u.Signatories)
                .Select(s => s.Subscriber)
                .ToArrayAsync(cancellationToken);

            var userPost = await _userPostRepository.Query
                .Where(p => p.Id == subscribeNotification.PostId)
                .Select(p => new
                {
                    p.Description,
                    p.PostCreatedAt,
                    p.CreatedUserProfile
                })
                .SingleAsync(cancellationToken);

            var postEmailModel = new PostEmailModel
            {
                Description = userPost.Description,
                PostCreatedAt = userPost.PostCreatedAt,
                UserProfile = _mapper.Map<UserProfileDto>(userPost.CreatedUserProfile)
            };

            foreach (var subscriber in subscribers)
            {
                var sendEmailModel = new EmailModel
                {
                    FirstName = subscriber.FirstName,
                    LastName = subscriber.LastName,
                    UserName = subscriber.UserName,
                    ShareLink = "#",
                    Post = postEmailModel
                };

                _emailSenderhelper.SendEmailMessage(sendEmailModel);
            }

            return Unit.Value;
        }
    }
}
