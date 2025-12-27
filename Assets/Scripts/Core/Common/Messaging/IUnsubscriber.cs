using System;

namespace Core.Common.Messaging
{
    public interface IUnsubscriber<T> : ISubscriber<T>
    {
        void Unsubscribe(Action<T> handler);
    }
}
