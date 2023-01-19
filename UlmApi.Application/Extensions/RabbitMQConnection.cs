using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace UlmApi.Application.Extensions
{
    public static class RabbitMQConnection
    {
        public static void AddRabbitMQConnection(this IServiceCollection services)
        {
            services.AddSingleton<IConnection>(serviceProvider =>
            {
                var userName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest";
                var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";
                var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
                var port = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
                var uri = $"amqp://{userName}:{password}@{host}:{port}/";

                var connectionFactory = new ConnectionFactory()
                {
                    Uri = new Uri(uri),
                    AutomaticRecoveryEnabled = true,
                    DispatchConsumersAsync = true
                };

                connectionFactory.UserName = userName;
                connectionFactory.Password = password;

                return connectionFactory.CreateConnection();
            });
        }
    }
}
