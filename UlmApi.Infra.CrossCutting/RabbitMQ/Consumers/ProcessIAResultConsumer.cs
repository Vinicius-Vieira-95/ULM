using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models;

namespace UlmApi.Infra.CrossCutting.RabbitMQ.Consumers
{
    public class ProcessIAResultConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private IServiceScopeFactory _serviceScopeFactory;

        public ProcessIAResultConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var factory = CreateConnectionFactory();
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: "queue_requests_processed_IA",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var payload = JsonConvert.DeserializeObject<UpdateIaFieldsModel>(contentString);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _requestLicenseService = scope.ServiceProvider.GetService<IRequestLicenseService>();
                    await _requestLicenseService.UpdateIaFields(payload);
                }

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume("queue_requests_processed_IA", false, consumer);

            return Task.CompletedTask;
        }

        private ConnectionFactory CreateConnectionFactory()
        {
            var userName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest";
            var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";
            var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
            var uri = $"amqp://{userName}:{password}@{host}:{port}/";

            return new ConnectionFactory()
            {
                Uri = new Uri(uri),
                UserName = userName,
                Password = password
            };
        }
    }

}