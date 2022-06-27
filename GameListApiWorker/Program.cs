using GameListApiWorker;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<RabbitMqMessageConsumer>()
                .Endpoint(e => e.Name = configuration["RabbitMqConfiguration:QueueName"]);

            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMqConfiguration:HostName"], "/", host =>
                {
                    host.Username(configuration["RabbitMqConfiguration:UserName"]);
                    host.Password(configuration["RabbitMqConfiguration:Password"]);
                });

                config.ConfigureEndpoints(context);
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
