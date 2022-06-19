using MassTransit;
using Newtonsoft.Json;

namespace GameListApiWorker
{
    public record TestMessage(string TestString, int TestInt);

    public class RabbitMqMessageConsumer : IConsumer<TestMessage>
    {
        public ILogger<RabbitMqMessageConsumer> Logger { get; }

        public RabbitMqMessageConsumer(ILogger<RabbitMqMessageConsumer> logger)
        {
            Logger = logger;
        }

        public Task Consume(ConsumeContext<TestMessage> context)
        {
            Logger.LogInformation("Message Consumed! Message: {message}", JsonConvert.SerializeObject(context.Message));
            return Task.CompletedTask;
        }
    }
}
