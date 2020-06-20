using System.Text;
using Newtonsoft.Json;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TheGramProfile.EventBus.Connection;

namespace TheGramProfile.EventBus.Channels
{
    public class RabbitMQQueueChannel : RabbitMQAbstractChannel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RabbitMQBaseConnection _connection;
        private readonly string _queueName;
        public RabbitMQQueueChannel(RabbitMQBaseConnection connection, string queueName)
        {
            _connection = connection;
            _queueName = queueName;
        }
        
        
        private void ReceivedEvent(object sender, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var props = ea.BasicProperties;

            switch (ea.RoutingKey)
            {
                default:
                    Logger.Info(ea.RoutingKey);
                    break;
            }
        }

        public override IModel DeclareChannel()
        {
            if (!_connection.IsConnected)
            {
                Logger.Error("No connection while creating consumer channels, retrying.");
                _connection.TryConnect();
            }

            ConsumerChannel = _connection.CreateModel();
            ConsumerChannel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(ConsumerChannel);

            consumer.Received += ReceivedEvent;
            ConsumerChannel.CallbackException += CallbackException;
            
            ConsumerChannel.BasicConsume(_queueName, autoAck: false, consumer: consumer);
            ConsumerChannel.CallbackException += CallbackException;
            Logger.Info("Channel for queue {0} has been created", _queueName);
            return ConsumerChannel;
        }
        
        private void SendResponse(object body,string queueName, IBasicProperties props, ulong tag)
        {
            var response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));
            ConsumerChannel.BasicPublish(
                exchange: "",
                routingKey: queueName, 
                basicProperties: props,
                body: response);
            
            ConsumerChannel.BasicAck(deliveryTag: tag,
                multiple: false);
        }


        public override void Dispose()
        {
            
        }
    }
}