using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB
{
    public abstract class GraphNotificationHandler<TNotification> : ShortBus.INotificationHandler<TNotification>, ShortBus.IAsyncNotificationHandler<TNotification>
    {

        public void Handle(TNotification notification)
        {
            HandleNotification(notification);
        }

        public Task HandleAsync(TNotification notification)
        {
            return Task.Run(() => HandleNotification(notification));
        }

        protected abstract void HandleNotification(TNotification notification);

    }
}
