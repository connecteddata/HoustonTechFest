using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Messaging
{
    public abstract class NotificationHandler<TMessage> : ShortBus.INotificationHandler<TMessage>, ShortBus.IAsyncNotificationHandler<TMessage>
        where TMessage : IMessage
    {

        protected abstract void HandleProxy(TMessage message);


        public void Handle(TMessage message)
        {
            HandleProxy(message);
        }

        public Task HandleAsync(TMessage message)
        {
            return System.Threading.Tasks.Task.Run(() => HandleProxy(message));
        }
    }
}
