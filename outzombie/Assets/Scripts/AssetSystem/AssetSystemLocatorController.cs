using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ServiceLocatorSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetSystem
{
    public class AssetSystemLocatorController
    {
        private static Dictionary<Type, object> _services = new();
        
        public static T Resolve<T>() where T : IAssetSystemInstance
        {
            return (T)_services[typeof(T)];
        }
        
        public static async UniTask<T> Register<T>(string key, Transform parent) where T : Object, IAssetSystemInstance
        {
            var assetSystemLocator = ServiceLocatorController.Resolve<AssetSystemLocator>();
            var assetHandler = assetSystemLocator.Resolve<AssetHandler>();
            var instance = await assetHandler.InstantiateAndBind<T>(key, parent, false);
            _services[typeof(T)] = instance;
            return instance;
        }
    }
}