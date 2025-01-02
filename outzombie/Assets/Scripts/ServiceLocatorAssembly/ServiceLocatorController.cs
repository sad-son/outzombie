using System;
using System.Collections.Generic;

namespace ServiceLocatorSystem
{
    public static class ServiceLocatorController
    {
        private static readonly Dictionary<Type, object> _services = new();
        
        public static T Resolve<T>() where T : class, IServiceLocator
        {
            return _services[typeof(T)] as T;
        }
        
        public static void Register<T>(T resolver) where T : IServiceLocator
        {
            _services[typeof(T)] = resolver;
        }
        
        public static void Unregister<T>() where T : IServiceLocator
        {
            if (_services[typeof(T)] is IDisposable disposable)
                disposable.Dispose();
            
            _services.Remove(typeof(T));
        }
    }
}