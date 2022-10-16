using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InstagramClone.Application.Helpers;
using InstagramClone.Application.Models;
using InstagramClone.Domain.Models;
using InstagramClone.EmailSender.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InstagramClone.EmailSender.Services
{
    public class ConsumeEmailSenderMessagingService : BackgroundService
    {
        private readonly ILogger<ConsumeEmailSenderMessagingService> _logger;
        private readonly IConfiguration _configuration;
        private readonly RabbitMQHelper _rabbitMqHelperService;
        private readonly IServiceScopeFactory _scopeFactory;

        private IModel _channel;
        private IConnection _connection;

        private const string ConfigKey = "RabbitMq:NotificationsSender:";

        public ConsumeEmailSenderMessagingService(
            ILogger<ConsumeEmailSenderMessagingService> logger,
            IConfiguration configuration,
            RabbitMQHelper rabbitMqHelperService,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _rabbitMqHelperService = rabbitMqHelperService;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("ConsumeEmailSenderMessagingService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("ConsumeEmailSenderMessagingService is stopping."));

            InitRabbitMq();

            return Task.CompletedTask;
        }

        private void InitRabbitMq()
        {
            var factory = _rabbitMqHelperService.GetConnectionFactory();

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var exchangeName = _configuration[ConfigKey + "exchangeName"];
            var queueName = _configuration[ConfigKey + "queue"];
            var deadLetterExchangeName = _configuration[ConfigKey + "deadLetterQueue"];
            var deadLetterQueueName = _configuration[ConfigKey + "deadLetterExchangeName"];

            _channel.ExchangeDeclare(exchangeName, "direct", true);
            _channel.ExchangeDeclare(deadLetterExchangeName, "fanout", true);

            var argsForQueue = new Dictionary<string, object> { { "x-dead-letter-exchange", deadLetterExchangeName } };
            var argsForDeadLetterQueue = new Dictionary<string, object> { { "x-dead-letter-exchange", exchangeName } };

            _channel.QueueDeclare(queueName, true, false, false, argsForQueue);
            _channel.QueueDeclare(deadLetterQueueName, true, false, false, argsForDeadLetterQueue);

            var glShareConsumer = new AsyncEventingBasicConsumer(_channel);
            glShareConsumer.Received += HandleEmailSenderMessageAsync;

            BindRoutingKeysToQueue(
                _channel,
                queueName,
                exchangeName,
                new string[] {
                    MessageRoutingKey.SubscribeNotification,
                });

            BindRoutingKeysToQueue(
                _channel,
                deadLetterQueueName,
                deadLetterExchangeName,
                new string[] {
                    MessageRoutingKey.SubscribeNotification,
                });

            _channel.BasicConsume(queueName, false, glShareConsumer);
        }

        private void BindRoutingKeysToQueue(IModel channelForBinding, string queueName, string exchangeName, string[] routingKeys)
        {
            foreach (var routingKey in routingKeys)
            {
                BindQueue(channelForBinding, queueName, exchangeName, routingKey);
            }
        }

        private void BindQueue(IModel channelForBinding, string queueName, string exchangeName, string routingKey)
        {
            channelForBinding.QueueBind(queueName, exchangeName, routingKey, null);
        }

        private async Task HandleEmailSenderMessageAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var body = eventArgs.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                _logger.LogDebug($"Message received: {eventArgs.RoutingKey}:{message}");

                try
                {
                    switch (eventArgs.RoutingKey)
                    {
                        case MessageRoutingKey.SubscribeNotification:
                            {
                                var patentsHandleMessageHandleMessageService = scope.ServiceProvider.GetRequiredService<IMediator>();
                                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                                var subscribeNotificationEventModel = JsonConvert.DeserializeObject<SubscribeNotification>(message);

                                await mediator.Send(new ProcessUserSubscriptionNotificationsCommand(subscribeNotificationEventModel));
                                _channel.BasicAck(eventArgs.DeliveryTag, false);
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error handling message{Environment.NewLine}{e}");
                    _channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }
            }

            await Task.Yield();
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}