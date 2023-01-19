namespace UlmApi.Infra.CrossCutting.RabbitMQ
{
    public interface IEventBus
    {
        void Publish(object payload, string queueName);
    }
}
