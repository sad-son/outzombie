using System;
using System.Collections.Generic;

namespace ServiceLocatorSystem
{
    public abstract class SystemLocatorBase<TQueryType> : IDisposable
    {
        private readonly Dictionary<Type, Func<IDisposable>> _services;

        protected SystemLocatorBase()
        {
            _services = new Dictionary<Type, Func<IDisposable>>();
            RegisterTypesInternal();
        }

        public virtual void Dispose()
        {
            foreach (var service in _services)
            {
                service.Value().Dispose();
            }
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
        
        protected void Register<T>(Func<T> resolver) where T : TQueryType, IDisposable
        {
            _services[typeof(T)] = () => resolver();
        }
    }
}