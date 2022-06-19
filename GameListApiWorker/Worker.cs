using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GameListApiWorker
{
    public class Worker : BackgroundService
    {
        private ILogger<Worker> Logger { get; }
        private IConfiguration Configuration { get; }

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
            var factory = new ConnectionFactory()
            {
                HostName = Configuration["RabbitMqHostName"]
            };
            factory.DispatchConsumersAsync = true;
            var queueName = "test-queue-local";
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queueName);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                //perform processing
                Logger.LogInformation($"Message Recieved: {{messageBody}}", JsonConvert.SerializeObject(message));
                channel.BasicAck(ea.DeliveryTag, false);
                await Task.Yield();
            };

            channel.BasicConsume(queueName, false, consumer);
        }
    }
}