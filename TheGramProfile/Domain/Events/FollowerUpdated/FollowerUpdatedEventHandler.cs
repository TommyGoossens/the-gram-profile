using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using TheGramProfile.EventBus;
using TheGramProfile.EventBus.Requests;
using TheGramProfile.Properties;

namespace TheGramProfile.Domain.Events.FollowerUpdated
{
    public class FollowerUpdatedEventHandler : INotificationHandler<FollowerUpdatedEvent>
    {
        private readonly RabbitMQPublishTopic _rabbitMQ = new RabbitMQPublishTopic(topicExchange: "topic_user",
            routingKey: RabbitMqChannels.TopicFollowerUpdate);

        public async Task Handle(FollowerUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _rabbitMQ.PublishMessageOnTopic(JsonConvert.SerializeObject(notification));
            _rabbitMQ.Dispose();
        }
    }
}