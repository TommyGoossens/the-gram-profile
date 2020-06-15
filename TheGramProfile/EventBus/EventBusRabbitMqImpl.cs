using System;
using System.Text;
using Newtonsoft.Json;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TheGramPost.EventBus;
using TheGramProfile.Properties;

namespace TheGramProfile.EventBus
{
    public class EventBusRabbitMqImpl : IEventBusService, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IRabbitMQPersistentConn _persistentConn;
        private readonly IServiceProvider _serviceProvider;
        private IModel _consumerChannel;
        private readonly string _queueName;

        public EventBusRabbitMqImpl(IRabbitMQPersistentConn persistentConn, IServiceProvider serviceProvider,
            string queueName)
        {
            _persistentConn = persistentConn ?? throw new ArgumentNullException(nameof(persistentConn));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _queueName = queueName;
        }
        
        public IModel CreateConsumerChannel()
        {
            if (!_persistentConn.IsConnected)
            {
                Logger.Error("No connection while creating consumer channels, retrying.");
                _persistentConn.TryConnect();
            }

            var channel = _persistentConn.CreateModel();
            channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(_queueName, autoAck: false, consumer: consumer);
            channel.CallbackException += CallbackException;
            Logger.Info("Channel for queue {0} has been created", _queueName);
            _consumerChannel = channel;
            return channel;
        }

        private void CallbackException(object sender, CallbackExceptionEventArgs ea)
        {
            Logger.Error("Channel for queue {0} has crashed", _queueName, ea.Exception);
            _consumerChannel.Dispose();
            _consumerChannel = CreateConsumerChannel();
        }

        private void ReceivedEvent(object sender, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var props = ea.BasicProperties;

            switch (ea.RoutingKey)
            {
                case RabbitMqChannels.GetPostPreviews:
                    Logger.Info(message);
                    break;
                default:
                Logger.Info(ea.RoutingKey);
                break;
            }
        }

        private void ReceivedUserPostPreviews(string message)
        {
            /*var query = new GetUserPostsPreviewQuery
            {
                UserId = userId
            };
            var scope = _serviceProvider.CreateScope();
            var mediatr = scope.ServiceProvider.GetService<IMediator>();
            var result = await mediatr.Send(query);
            scope.Dispose();

            var channel = _persistentConn.CreateModel();
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;
            SendResponse(result,props.ReplyTo,replyProps,tag);*/
        }

        private void SendResponse(object body,string queueName, IBasicProperties props, ulong tag)
        {
            var response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));
            _consumerChannel.BasicPublish(
                exchange: "",
                routingKey: queueName, 
                basicProperties: props,
                body: response);
            
            _consumerChannel.BasicAck(deliveryTag: tag,
                multiple: false);
        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();
        }
    }
}