using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Common
{
    /// <summary>
    /// インスタンスを提供する
    /// </summary>
    public interface IInstanceProvider
    {
        object SpawnInstance(IServiceResolver resolver);
    }
    
    /// <summary>
    /// TypeからInstanceを生成するだけ
    /// </summary>
    public class ServiceReflectionProvider : IInstanceProvider
    {
        private readonly System.Type _implementationType;
        
        public ServiceReflectionProvider(System.Type implementationType)
        {
            _implementationType = implementationType;
        }

        public object SpawnInstance(IServiceResolver resolver)
        {
            return Activator.CreateInstance(_implementationType);
        }
    }

    /// <summary>
    /// SceneからFindして見つける
    /// </summary>
    public class ServiceSceneFindProvider : IInstanceProvider
    {
        private Scene _scene;
        private readonly Type _componentType;

        public ServiceSceneFindProvider(Scene scene, Type componentType)
        {
            _scene = scene;
            _componentType = componentType;
        }
        
        public object SpawnInstance(IServiceResolver resolver)
        {
            var gameObjects = new List<GameObject>();
            _scene.GetRootGameObjects(gameObjects);
            
            foreach (var gameObject in gameObjects)
            {
                var component = gameObject.GetComponent(_componentType);
                if (component)
                {
                    return component;
                }
            }

            return null; // Exceptionでいい
        }
    }
}