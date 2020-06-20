using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TheGramProfile.EventBus.Connection;

namespace TheGramProfile.EventBus.Channels
{
    public class RabbitMQExchangeChannel : RabbitMQAbstractChannel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RabbitMQBaseConnection _connection;
        private readonly string _exchangeTopic;
        private readonly IEnumerable<string> _routingKeys;

        public RabbitMQExchangeChannel(RabbitMQBaseConnection connection,string exchangeTopic, string[] routingKeys)
        {
            _connection = connection;
            _exchangeTopic = exchangeTopic;
            _routingKeys = routingKeys;
        }
        
        public override IModel DeclareChannel()
        {
            ConsumerChannel = _connection.CreateModel();
            ConsumerChannel.ExchangeDeclare(exchange:_exchangeTopic, type: ExchangeType.Topic);
            var queueName = ConsumerChannel.QueueDeclare().QueueName;
            foreach (var key in _routingKeys)
            {
                Console.WriteLine(key);
                ConsumerChannel.QueueBind(queue:queueName,exchange:_exchangeTopic,routingKey:key);
            }
            var consumer = new EventingBasicConsumer(ConsumerChannel);
            consumer.Received += ReceivedEvent;
            ConsumerChannel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return ConsumerChannel;
        }
        
        private void ReceivedEvent(object? sender, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Logger.Info("[Message received ]: {0}",message);
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}