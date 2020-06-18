using System;
using System.Collections.Concurrent;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TheGramPost.EventBus;
using TheGramProfile.EventBus;

namespace TheGramProfile.Services
{
    public class EventBusRabbitMqImpl<T> : IEventBusService<T>
    {
        private const string RoutingKey = "queue_rpc_get_posts";

        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private readonly BlockingCollection<T> _respQueue = new BlockingCollection<T>();
        private readonly IBasicProperties _props;
        private readonly string _correlationId;
        public EventBusRabbitMqImpl(string hostname)
        {
            // Establish connection with the RabbitMQ instance
            var factory = new ConnectionFactory() {HostName = hostname};
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            // Create callback queue
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _props = _channel.CreateBasicProperties();
            _correlationId = Guid.NewGuid().ToString();
            _props.CorrelationId = _correlationId;
            _props.ReplyTo = _replyQueueName;

            _consumer.Received += OnConsumerReceive;
        }

        private async void OnConsumerReceive(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var response = Encoding.UTF8.GetString(body.ToArray());
                            
            if (ea.BasicProperties.CorrelationId == _correlationId)
            {
                _respQueue.Add(JsonConvert.DeserializeObject<T>(response));
            }
                        
        }
        
        public T MakeRemoteCall(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "",
                routingKey: RoutingKey,
                basicProperties: _props,
                body: messageBytes);

            _channel.BasicConsume(
                consumer: _consumer,
                queue: _replyQueueName,
                autoAck: true);

            return _respQueue.Take();
        }

        public string PublishMessage()
        {
            throw new NotImplementedException();
        }

        public IModel CreateConsumerChannel()
        {
            throw new NotImplementedException();
        }
    }
}