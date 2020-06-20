using System.Text;
using RabbitMQ.Client;
using TheGramProfile.EventBus.Connection;

namespace TheGramProfile.EventBus.Requests
{
    public class RabbitMQPublishTopic : RabbitMQScopedConnection
    {
        private readonly string _routingKey;
        private readonly string _exchange;

        public RabbitMQPublishTopic(string topicExchange, string routingKey)
        {
            Channel.ExchangeDeclare(exchange: topicExchange, type: "topic");
            _routingKey = routingKey;
            _exchange = topicExchange;
        }

        public void PublishMessageOnTopic(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            Channel.BasicPublish(exchange: _exchange, routingKey: _routingKey, basicProperties: null, body: body);
        }

        public override void Dispose()
        {
            Channel?.Dispose();
        }
    }
}