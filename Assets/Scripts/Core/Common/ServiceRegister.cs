using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Common
{
    /// <summary>
    /// GameObjectについているScriptをRegisterする際に利用する
    /// </summary>
    [DefaultExecutionOrder(-5000)]
    public class ServiceRegister : MonoBehaviour
    {
        [SerializeField] private GameObject[] _registerComponents;
        [SerializeField] private GameObject[] _autoInjectComponents;

        private readonly List<System.Type> _registeredTypes = new();
        private readonly IInjector _injector = new ReflectionInjector();

        public void InjectAutoGameObject()
        {
            if (_autoInjectComponents == null) return;
            
            foreach (var instance in _autoInjectComponents)
            {
                InjectGameObject(instance);
            }
        }
        
        protected virtual void RegisterService()
        {
        }

        protected void RegisterComponentInHierarchy<T>()
        {
            RegisterComponentInHierarchy(typeof(T));
        }

        protected void RegisterComponentInHierarchy(System.Type type)
        {
            var scene = gameObject.scene;

            var sceneFindProvider = new ServiceSceneFindProvider(scene, type);
            var registration = new Registration(
                type,
                Lifetime.Singleton,
                sceneFindProvider);
            
            TrackAndRegister(type, registration);
        }

        protected void RegisterSingleton<TInterface, TImpl>()
            where TImpl : TInterface
        {
            RegisterSingleton(typeof(TInterface), typeof(TImpl));
        }

        protected void RegisterSingleton<TInterface1, TInterface2, TImpl>()
            where TImpl : TInterface1, TInterface2
        {
            RegisterSingleton(typeof(TInterface1), typeof(TImpl));
            RegisterSingleton(typeof(TInterface2), typeof(TImpl));
        }

        protected void RegisterSingleton(Type type, Type impl)
        {
            var registration = new Registration(
                impl,
                Lifetime.Singleton,
                new ServiceReflectionProvider(impl));
            TrackAndRegister(type, registration);
        }
        
        private void Register()
        {
            RegisterService();
            RegisterComponentsInHierarchy();
        }

        private void RegisterComponentsInHierarchy()
        {
            foreach (var component in _registerComponents)
            {
                // componentに含まれているすべてのScriptを取得
                var components = component.GetComponents<Component>();
                
                // Interfaceを継承していたらそれを登録する
                foreach (var c in components)
                {
                    // Transformは除外
                    if (c is Transform) continue;
                    
                    var type = c.GetType();
                    if (type.IsAbstract || type.IsGenericTypeDefinition) continue;
                    
                    // Interfaceの型を取得
                    var interfaces = type.GetInterfaces();
                    if (interfaces.Length == 0) continue;
                    
                    // Interfaceを継承している場合は、Registerする
                    foreach (var i in interfaces)
                    {   
                        RegisterComponentInHierarchy(i);
                    }
                }
            }
        }

        private void InjectGameObject(GameObject instance)
        {
            void InjectGameObjectRecursive(GameObject current)
            {
                if (current == null) return;

                var components = current.GetComponents<MonoBehaviour>();
                foreach (var component in components)
                {
                    if (component is null) continue;
                    
                    _injector.Inject(component, ServiceLocator.Resolver);
                }
                
                for (var i = 0; i < current.transform.childCount; i++)
                {
                    var child = current.transform.GetChild(i);
                    InjectGameObjectRecursive(child.gameObject);
                }
            }

            InjectGameObjectRecursive(instance);
        }
        
        private void Awake()
        {
            Register();
        }

        private void OnDestroy()
        {
            // 登録した型を解除し、生成済みSingletonはDispose
            foreach (var type in _registeredTypes)
            {
                ServiceLocator.Unregister(type);
            }
        }

        // RegisterComponentInHierarchyから呼ばれる共通登録処理
        protected void TrackAndRegister(System.Type type, Registration registration)
        {
            ServiceLocator.Register(type, registration);
            _registeredTypes.Add(type);
        }
    }
}
