using System;

namespace InstagramClone.Application.Services.RabbitMQ
{
    public interface IRabbitMQEmailSenderService
    {
        void Send(ReadOnlyMemory<byte> message);
    }
}
