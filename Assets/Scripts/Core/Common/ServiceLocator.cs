using System;

namespace Core.Common
{
    public enum Lifetime
    {
        Transient,  // 取得時に新しいインスタンスを返す
        Singleton,  // 常に同じインスタンスを利用する
    }

    /// <summary>
    /// ServiceLocatorパターン
    /// 
    /// ※ DIを理解するための勉強用に利用しています。アンチパターンなので、実際のプロダクションコードでは使用しない前提でお願いします
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceRegister serviceRegister;
        private static IServiceResolver serviceResolver;

        static ServiceLocator()
        {
            var container = new Container();
            serviceRegister = container;
            serviceResolver = container;
        }
        
        public static IServiceResolver Resolver => serviceResolver;

        /// <summary>
        /// Interfaceに対する依存クラスの登録
        /// </summary>
        public static void Register<TInterface, T>(Lifetime lifetime) where T : TInterface
        {
            serviceRegister.Register<TInterface>(typeof(T), lifetime);
        }

        public static void Register(Type type, Type implementationType, Lifetime lifetime)
        {
            serviceRegister.Register(type, implementationType, lifetime);
        }
        public static void Register(Type typeA, Type typeB, Type implementationType, Lifetime lifetime)
        {
            serviceRegister.Register(typeA, typeB, implementationType, lifetime);
        }

        public static void Register(Type type, Registration registration)
        {
            serviceRegister.Register(type, registration);
        }

        public static void Unregister(Type type)
        {
            serviceRegister.Unregister(type);
        }
        
        public static void Unregister<T>()
        {
            serviceRegister.Unregister(typeof(T));
        }
        
        /// <summary>
        /// 依存関係の解決
        /// </summary>
        public static T Resolve<T>() =>
            (T)serviceResolver.Resolve(typeof(T));
    }
}
