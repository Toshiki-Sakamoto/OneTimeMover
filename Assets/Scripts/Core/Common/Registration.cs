using System;
using System.Collections.Generic;

namespace Core.Common
{
    /// <summary>
    /// 解決方法の管理(意味をあまりなしてない
    /// </summary>
    public class Registration
    {
        public readonly Type ImplementationType;
        public readonly Lifetime Lifetime;
        public readonly IInstanceProvider Provider;

        public Registration(
            Type implementationType,
            Lifetime lifetime,
            IInstanceProvider provider)
        {
            ImplementationType = implementationType;
            Lifetime = lifetime;
            Provider = provider;
        }
        
        public object SpawnInstance(IServiceResolver resolver) =>
            Provider.SpawnInstance(resolver);
    }
}