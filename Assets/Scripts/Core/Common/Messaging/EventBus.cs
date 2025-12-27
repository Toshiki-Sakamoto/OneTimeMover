using System;
using System.Collections.Generic;

namespace Core.Common.Messaging
{
    /// <summary>
    /// ただイベントを受け流すだけのメッセージバス
    /// </summary>
    public class EventBus<T> : IPublisher<T>, ISubscriber<T>
    {
        private readonly List<Action<T>> _subscribers = new();

        public void Publish(T message)
        {
            foreach (var sub in _subscribers)
            {
                sub.Invoke(message);
            }
        }

        public void Subscribe(Action<T> handler)
            => _subscribers.Add(handler);
    }
}