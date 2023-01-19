using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace UlmApi.Infra.CrossCutting.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IConnection _connection;

        public EventBusRabbitMQ(IConnection connection)
        {
            _connection = connection;
        }

        public void Publish(object payload, string queueName)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var jsonOptions = new JsonSerializerSettings()
                { 
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                };
                var message = JsonConvert.SerializeObject(payload, Formatting.Indented, jsonOptions);
                var bytesMessage = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: bytesMessage
                );
            }

        }
    }
}