using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TheGramProfile.EventBus.Requests
{
    public class RabbitMQRemoteProcedureCall<T> : Connection.RabbitMQScopedConnection
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _queueName;
        private string _replyQueueName;
        private EventingBasicConsumer _consumer;
        private IBasicProperties _props;
        private readonly BlockingCollection<T> _respQueue = new BlockingCollection<T>();
        private readonly TimeSpan _rpcTimeOut = TimeSpan.FromSeconds(2);


        public RabbitMQRemoteProcedureCall(string queueName)
        {
            _queueName = queueName;
            SetupConnection();
            _consumer.Received += OnReceived;
        }
        
        private void OnReceived(object sender, BasicDeliverEventArgs ea)
        {
            if (!ea.BasicProperties.CorrelationId.Equals(_props.CorrelationId)) return;

            var response = ea.Body;
            var body = Encoding.UTF8.GetString(response.ToArray());
            var parsed = JsonConvert.DeserializeObject<T>(body);
            _respQueue.Add(parsed);
        }

        private void SetupConnection()
        {
            _replyQueueName = Channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(Channel);
            _props = Channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _props.CorrelationId = correlationId;
            _props.ReplyTo = _replyQueueName;
        }

        public T MakeRemoteProcedureCall<T>(string requestBody, CancellationToken cancellationToken) where T : class, new()
        {
            var messageBytes = Encoding.UTF8.GetBytes(requestBody);
            Channel.BasicPublish("", _queueName, _props, messageBytes);
            Channel.BasicConsume(_consumer, _replyQueueName, true);
            var task = Task.Run(() => _respQueue.Take() as T, cancellationToken);
            if (task.Wait(_rpcTimeOut))
            {
                _logger.Info("[Response received on queue] : {0}",_replyQueueName);
                return task.Result;
            }
            _logger.Error("Connection timed out after 2 seconds on channel {0}", _queueName);
            return new T();
        }

        public override void Dispose()
        {
            Channel?.Dispose();
            _respQueue?.Dispose();
            Disconnect();
        }
    }
}