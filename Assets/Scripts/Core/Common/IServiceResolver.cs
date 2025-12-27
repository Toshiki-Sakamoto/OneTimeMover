using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Core.Common
{
    public interface IServiceResolver
    {
        /// <summary>
        /// 型から登録されているインスタンスを取得
        /// </summary>
        object Resolve(Type type);

        IEnumerable<T> Resolve<T>();
    }

    public interface IServiceRegister
    {
        void Register<T>(Type implementationType, Lifetime lifetime);
        
        void Register(Type type, Type implementationType, Lifetime lifetime);
        void Register(Type typeA, Type typeB, Type implementationType, Lifetime lifetime);

        void Register(Type type, Registration registration);

        /// <summary>
        /// 登録解除（Singletonの生成済みインスタンスがあればDisposeを試みる）
        /// </summary>
        void Unregister(Type type);
    }

    public class Container : IServiceResolver, IServiceRegister
    {
        private readonly ConcurrentDictionary<Type, Registration> _registry = new();
        private readonly ConcurrentDictionary<Registration, Lazy<object>> _sharedInstance = new();  // Singleton
        private readonly System.Func<Registration, Lazy<object>> _createInstance;   // Transient


        public Container()
        {
            _createInstance = registration =>
            {
                return new Lazy<object>(() => registration.SpawnInstance(this));
            };
        }

        public void Register<T>(Type implementationType, Lifetime lifetime) =>
            Register(typeof(T), implementationType, lifetime);

        public void Register(Type type, Type implementationType, Lifetime lifetime)
        {
            // 生成者
            var spawner = new ServiceReflectionProvider(implementationType);
            
            var registration = new Registration(
                implementationType,
                lifetime,
                spawner);
            
            _registry.TryAdd(type, registration);
        }

        public void Register(Type typeA, Type typeB, Type implementationType, Lifetime lifetime)
        {
            
            var spawner = new ServiceReflectionProvider(implementationType);
            
            var registration = new Registration(
                implementationType,
                lifetime,
                spawner);
            
            _registry.TryAdd(typeA, registration);
            _registry.TryAdd(typeB, registration);
        }

        public void Register(Type type, Registration registration)
        {
            _registry.TryAdd(type, registration);
        }

        public void Unregister(Type type)
        {
            if (type == null) return;

            if (_registry.TryRemove(type, out var registration))
            {
                if (_sharedInstance.TryRemove(registration, out var lazyInstance))
                {
                    if (lazyInstance.IsValueCreated && lazyInstance.Value is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }
        
        public object Resolve(Type type)
        {
            // 配列要求に対応：要素型を実装する全登録を列挙して配列を返す
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var matches = _registry.Values
                    .Where(reg => elementType != null && elementType.IsAssignableFrom(reg.ImplementationType))
                    .Select(Resolve)
                    .ToList();

                var array = Array.CreateInstance(elementType ?? typeof(object), matches.Count);
                for (var i = 0; i < matches.Count; i++)
                {
                    array.SetValue(matches[i], i);
                }
                return array;
            }

            if (TryGetRegistration(type, out var registration))
            {
                return Resolve(registration);
            }
            
            throw new InvalidOperationException($"Type {type} is not registered.");
        }

        public bool TryGetRegistration(Type type, out Registration registration)
            => _registry.TryGetValue(type, out registration);

        public object Resolve(Registration registration) =>
            ResolveCore(registration);
        
        public IEnumerable<T> Resolve<T>()
        {
            return _registry.Values
                .Where(registration => typeof(T).IsAssignableFrom(registration.ImplementationType))
                .Select(Resolve)
                .Cast<T>();
        }
        
        private object ResolveCore(Registration registration)
        {
            switch (registration.Lifetime)
            {
                // インスタンスは一回だけ生成
                case Lifetime.Singleton:
                    var instance = _sharedInstance.GetOrAdd(registration, _createInstance);
                    return instance.Value;
                
                // 常に生成
                case Lifetime.Transient:
                    return registration.SpawnInstance(this);
                
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(registration.Lifetime),
                        registration.Lifetime,
                        "Invalid lifetime");
            }
        }
    }
}
