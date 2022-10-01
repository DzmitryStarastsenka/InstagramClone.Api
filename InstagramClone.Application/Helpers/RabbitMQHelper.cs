using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace InstagramClone.Application.Helpers
{
    public class RabbitMQHelper
    {
        private readonly IConfiguration _configuration;

        public RabbitMQHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ConnectionFactory GetConnectionFactory()
        {
            var rabbitMQConfig = "RabbitMQ:";

            return new ConnectionFactory
            {
                HostName = _configuration[rabbitMQConfig + "host"],
                UserName = _configuration[rabbitMQConfig + "username"],
                Port = int.Parse(_configuration[rabbitMQConfig + "port"]),
                Password = _configuration[rabbitMQConfig + "password"],
                RequestedHeartbeat = new TimeSpan(60),
                AutomaticRecoveryEnabled = true,
                DispatchConsumersAsync = true,
            };
        }
    }
}
