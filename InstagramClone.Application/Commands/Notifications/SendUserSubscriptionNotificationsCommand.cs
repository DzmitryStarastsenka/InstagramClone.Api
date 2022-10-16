using InstagramClone.Application.Models;
using InstagramClone.Application.Services.RabbitMQ;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Commands.Notifications
{
    public class SendUserSubscriptionNotificationsCommand : IRequest<Unit>
    {
        public SendUserSubscriptionNotificationsCommand(SubscribeNotification subscribeNotification)
        {
            SubscribeNotification = subscribeNotification;
        }

        public SubscribeNotification SubscribeNotification { get; }
    }

    public class SendUserSubscriptionNotificationsCommandHandler : IRequestHandler<SendUserSubscriptionNotificationsCommand, Unit>
    {
        private readonly ILogger<SendUserSubscriptionNotificationsCommandHandler> _logger;
        private readonly IRabbitMQEmailSenderService _rabbitMQEmailSenderService;

        public SendUserSubscriptionNotificationsCommandHandler(
            ILogger<SendUserSubscriptionNotificationsCommandHandler> logger,
            IRabbitMQEmailSenderService rabbitMQEmailSenderService)
        {
            _logger = logger;
            _rabbitMQEmailSenderService = rabbitMQEmailSenderService;
        }

        public Task<Unit> Handle(SendUserSubscriptionNotificationsCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Send notifications through RabbitMQ");

            _rabbitMQEmailSenderService.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command.SubscribeNotification)));

            return Task.FromResult(Unit.Value);
        }
    }
}
