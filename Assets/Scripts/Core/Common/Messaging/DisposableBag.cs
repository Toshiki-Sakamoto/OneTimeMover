using System;
using System.Collections.Generic;

namespace Core.Common.Messaging
{
    /// <summary>
    /// 簡易CompositeDisposable
    /// </summary>
    public sealed class DisposableBag : IDisposable
    {
        private readonly List<IDisposable> _items = new();
        private bool _disposed;

        public void Add(IDisposable disposable)
        {
            if (disposable == null || _disposed) return;
            _items.Add(disposable);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            foreach (var d in _items)
            {
                d?.Dispose();
            }
            _items.Clear();
        }
    }
}
