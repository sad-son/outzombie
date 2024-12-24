using System;
using System.Collections.Generic;

namespace ServiceLocatorSystem
{
    public abstract class SystemLocatorBase<TQueryType>
    {
        private readonly Dictionary<Type, Func<object>> _services;

        protected SystemLocatorBase()
        {
            _services = new Dictionary<Type, Func<object>>();
            RegisterTypesInternal();
        }

        private void RegisterTypesInternal()
        {
            RegisterTypes();
        }
        
        protected abstract void RegisterTypes();
        
        public T Resolve<T>() where T : TQueryType
        {
            return (T)_services[typeof(T)]();
        }
        
        protected void Register<T>(Func<T> resolver) where T : TQueryType
        {
            _services[typeof(T)] = () => resolver();
        }
    }
}