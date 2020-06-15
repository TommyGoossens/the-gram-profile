using System;
using System.IO;
using System.Threading;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using TheGramProfile.EventBus;
using TheGramProfile.Properties;

namespace TheGramPost.EventBus
{
    public class RabbitMqPersistentConn : IRabbitMQPersistentConn
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConnectionFactory _connectionFactory;
        private readonly IServiceProvider _serviceProvider;
        private IEventBusService _eventBusService;
        private IConnection _connection;
        private bool _isDisposed;
        public bool IsConnected => _connection != null && _connection.IsOpen && !_isDisposed;

        public RabbitMqPersistentConn(IServiceProvider serviceProvider)
        {
            //_connectionFactory = factory ?? throw new ArgumentNullException(nameof(factory));

            _connectionFactory = new ConnectionFactory()
            {
                HostName = "rabbitmq-service",
                Port = 7000
            };
            _serviceProvider = serviceProvider;
            if (!IsConnected)
            {
                TryConnect();
            }
        }


        public bool TryConnect()
        {
            try
            {
                Logger.Info("RabbitMQ is connecting");
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Logger.Error("RabbitMQ couldn't connect, retrying in 5 seconds", e);
                Thread.Sleep(5000);
                _connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                Logger.Info(
                    "RabbitMQ persistent connection acquired a connection {0} and is subscribed to failure events",
                    _connection.Endpoint.HostName);
                return true;
            }
            else
            {
                Logger.Fatal("RabbitMQ connections could not be created and opened");
                return false;
            }
        }

        public IModel CreateModel()
        {
            if (IsConnected) return _connection.CreateModel();
            Logger.Error("No RabbitMQ connections are available to allow the creation of a model");
            throw new InvalidOperationException(
                "No RabbitMQ connections are available to allow the creation of a model");
        }

        public void CreateConsumerChannel()
        {
            if (!IsConnected)
            {
                Logger.Error("No connection while creating consumer channels, retrying.");
                TryConnect();
            }

            _eventBusService = new EventBusRabbitMqImpl(this, _serviceProvider, RabbitMqChannels.GetPostPreviews);
            _eventBusService.CreateConsumerChannel();
        }

        public void Disconnect()
        {
            if (_isDisposed)
            {
                Logger.Error("Connection was already disposed when disconnecting.");
                return;
            }

            Dispose();
        }

        private void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_isDisposed) return;
            Console.WriteLine("A RabbitMQ connection is shutdown. Trying to re-connect...");
            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_isDisposed) return;
            Console.WriteLine("A RabbitMQ connection throw exception. Trying to re-connect...");
            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_isDisposed) return;
            Console.WriteLine("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            TryConnect();
        }
    }
}