using RabbitMQ.Client;

namespace TheGramProfile.EventBus
{
    public interface IEventBusService
    {
        public IModel CreateConsumerChannel();
    }
}