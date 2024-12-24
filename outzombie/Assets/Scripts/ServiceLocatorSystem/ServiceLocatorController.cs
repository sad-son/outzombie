using System;
using System.Collections.Generic;

namespace ServiceLocatorSystem
{
    public static class ServiceLocatorController
    {
        private static Dictionary<Type, Func<object>> _services = new();
        
        public static T Resolve<T>() where T : IServiceLocator
        {
            return (T)_services[typeof(T)]();
        }
        
        public static void Register<T>(Func<T> resolver) where T : IServiceLocator
        {
            _services[typeof(T)] = () => resolver();
        }
    }
}