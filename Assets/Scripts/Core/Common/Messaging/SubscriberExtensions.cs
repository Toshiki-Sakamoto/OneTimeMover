using System;
using UnityEngine;

namespace Core.Common.Messaging
{
    public static class SubscriberExtensions
    {
        /// <summary>
        /// 購読解除可能なIDisposableを返すSubscribe。
        /// </summary>
        public static IDisposable SubscribeDisposable<T>(this ISubscriber<T> subscriber, Action<T> handler)
        {
            subscriber.Subscribe(handler);
            return new ActionDisposable(() => UnsubscribeIfPossible(subscriber, handler));
        }

        /// <summary>
        /// IDisposableをバッグに追加。
        /// </summary>
        public static void AddTo(this IDisposable disposable, DisposableBag bag)
        {
            bag?.Add(disposable);
        }

        /// <summary>
        /// MonoBehaviourのライフサイクルに紐づけて、OnDestroy時にDisposeする。
        /// </summary>
        public static void AddTo(this IDisposable disposable, MonoBehaviour owner)
        {
            if (disposable == null || owner == null) return;

            var hook = owner.GetComponent<DisposableHook>();
            if (hook == null)
            {
                hook = owner.gameObject.AddComponent<DisposableHook>();
            }

            hook.Add(disposable);
        }

        private static void UnsubscribeIfPossible<T>(ISubscriber<T> subscriber, Action<T> handler)
        {
            if (subscriber is IUnsubscriber<T> unsub)
            {
                unsub.Unsubscribe(handler);
            }
        }

        private sealed class ActionDisposable : IDisposable
        {
            private Action _dispose;
            public ActionDisposable(Action dispose) => _dispose = dispose;
            public void Dispose()
            {
                _dispose?.Invoke();
                _dispose = null;
            }
        }

        /// <summary>
        /// MonoBehaviour破棄時に保持したDisposableをまとめてDisposeするフック。
        /// </summary>
        private sealed class DisposableHook : MonoBehaviour
        {
            private readonly System.Collections.Generic.List<IDisposable> _items = new();

            public void Add(IDisposable disposable)
            {
                if (disposable == null) return;
                _items.Add(disposable);
            }

            private void OnDestroy()
            {
                foreach (var d in _items)
                {
                    d?.Dispose();
                }
                _items.Clear();
            }
        }
    }
}
