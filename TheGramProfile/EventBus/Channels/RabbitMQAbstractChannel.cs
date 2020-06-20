using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TheGramProfile.EventBus.Channels
{
    public abstract class RabbitMQAbstractChannel : IDisposable
    {
        protected IModel ConsumerChannel;
        public abstract void Dispose();

        public abstract IModel DeclareChannel();
        
        protected void CallbackException(object sender, CallbackExceptionEventArgs ea)
        {
            /*Logger.Error( ea.Exception,"Channel for queue has crashed");*/
            ConsumerChannel.Dispose();
            ConsumerChannel = DeclareChannel();
        }
    }
}