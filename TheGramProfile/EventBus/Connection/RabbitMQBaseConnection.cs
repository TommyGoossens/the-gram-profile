using System;
using System.IO;
using System.Threading;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using TheGramProfile.Properties;

namespace TheGramProfile.EventBus.Connection
{
    public abstract class RabbitMQBaseConnection : IRabbitMQBaseConnection
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public bool IsConnected => Connection != null && Connection.IsOpen && !_isDisposed;
        
        private bool _isDisposed;
        private readonly IConnectionFactory _connectionFactory;
        protected IConnection Connection;
        
        protected RabbitMQBaseConnection()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = Constants.RabbitMQHost,
                Port = Constants.RabbitMQPort
            };
            if (!IsConnected)
            {
                TryConnect();
            }
        }
        
        public IModel CreateModel()
        {
            if (IsConnected) return Connection.CreateModel();
            Logger.Error("No RabbitMQ connections are available to allow the creation of a model");
            throw new InvalidOperationException(
                "No RabbitMQ connections are available to allow the creation of a model");
        }
        
        public bool TryConnect()
        {
            try
            {
                Logger.Info("RabbitMQ is connecting");
                Connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Logger.Error("RabbitMQ couldn't connect, retrying in 5 seconds", e);
                Thread.Sleep(5000);
                Connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected)
            {
                Connection.ConnectionShutdown += OnConnectionShutdown;
                Connection.CallbackException += OnCallbackException;
                Connection.ConnectionBlocked += OnConnectionBlocked;

                Logger.Info(
                    "RabbitMQ persistent connection acquired a connection {0} and is subscribed to failure events",
                    Connection.Endpoint.HostName);
                return true;
            }

            Logger.Fatal("RabbitMQ connections could not be created and opened");
            return false;
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
                Connection.Dispose();
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

    public interface IRabbitMQBaseConnection
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
        
        void Disconnect();
    }
}