using System;
using System.Collections.Concurrent;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TheGramProfile.EventBus
{
    public class RabbitRPC<T> : IDisposable
    {
        private readonly IModel _channel;
        private readonly string _queueName;
        private string _replyQueueName;
        private EventingBasicConsumer _consumer;
        private IBasicProperties _props;
        private readonly BlockingCollection<T> _respQueue = new BlockingCollection<T>();


        public RabbitRPC(string queueName)
        {
            var factory = new ConnectionFactory(){HostName = "localhost"};
            var conn = factory.CreateConnection();
            var channel = conn.CreateModel();
            _queueName = queueName;
            _channel = channel;
            SetupConnection();

            _consumer.Received += OnReceived;
        }

        private void OnReceived(object sender, BasicDeliverEventArgs ea)
        {
            if (!ea.BasicProperties.CorrelationId.Equals(_props.CorrelationId)) return;
            
            var response = ea.Body;
            var  body = Encoding.UTF8.GetString(response.ToArray());
            var parsed = JsonConvert.DeserializeObject<T>(body);
            _respQueue.Add(parsed);
        }

        private void SetupConnection()
        {
            _replyQueueName = IModelExensions.QueueDeclare(_channel).QueueName;
            _consumer = new EventingBasicConsumer(_channel);
            _props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _props.CorrelationId = correlationId;
            _props.ReplyTo = _replyQueueName;
        }

        public T Request<T>(string requestBody) where T : class
        {
            var messageBytes = Encoding.UTF8.GetBytes(requestBody);
            _channel.BasicPublish("",_queueName,_props,messageBytes);
            _channel.BasicConsume(_consumer, _replyQueueName, true);
            return _respQueue.Take() as T;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _respQueue?.Dispose();
        }
    }
}