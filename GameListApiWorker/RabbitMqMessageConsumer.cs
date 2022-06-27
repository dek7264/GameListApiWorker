using GameListApiWorker.Contracts;
using MassTransit;
using Newtonsoft.Json;

namespace GameListApiWorker
{
    public class RabbitMqMessageConsumer : IConsumer<RabbitMqMessage>
    {
        public ILogger<RabbitMqMessageConsumer> Logger { get; }

        public RabbitMqMessageConsumer(ILogger<RabbitMqMessageConsumer> logger)
        {
            Logger = logger;
        }

        public Task Consume(ConsumeContext<RabbitMqMessage> context)
        {
            Logger.LogInformation("Message Consumed! Message: {message}", JsonConvert.SerializeObject(context.Message));
            return Task.CompletedTask;
        }
    }
}
