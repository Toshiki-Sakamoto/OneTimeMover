using System;
using System.Linq;
using System.Reflection;
using Core.Common.Messaging;
using UnityEngine;

namespace Core.Common
{
    public static class ServiceLocatorExtensions
    {
        /// <summary>
        /// 末尾Eventとついてるもの全てを登録する便利メソッド
        /// ※Roslynを使ってクラスを自動生成するのが本来めっちゃいい
        /// </summary>
        public static void RegisterAllEventBuses()
        {
            // ロード済みアセンブリをすべて取得し、システム系を除外
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a =>
                {
                    var name = a.GetName().Name;
                    return !name.StartsWith("Unity") 
                           && !name.StartsWith("System") 
                           && !name.StartsWith("mscorlib");
                });
            
            // 末尾が "Event" のクラスをすべて抽出
            // Note: IPublisher<> と ISubscriber<> を実装しているものを抽出のがいい
            var eventTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Event"));
            
            foreach (var evt in eventTypes)
            {
                var pub   = typeof(IPublisher<>).MakeGenericType(evt);
                var sub   = typeof(ISubscriber<>).MakeGenericType(evt);
                var bus   = typeof(EventBus<>).MakeGenericType(evt);
                ServiceLocator.Register(pub, sub, bus, Lifetime.Singleton);
            }
        }
    }
}