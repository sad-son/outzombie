using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace AssetSystem
{
    public class AssetHandler : IAssetSystemInstance, IDisposable
    {
        public async UniTask<(AsyncOperationHandle<GameObject> asyncOperationHandle, GameObject asset)> LoadAssetAsync(object key)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(key);
            var asset = await asyncOperationHandle.Task.AsUniTask();
            return (asyncOperationHandle, asset);
        }

        public async UniTask<T> InstantiateAndBind<T>(object key, Transform parent, bool instantiateInWorldSpace) where T : Object
        {
            var instance = await Instantiate(key, parent, instantiateInWorldSpace);
            instance.BindToGameObjectLifeTime().Forget();
            return instance.GetComponent<T>();
        }

        public async UniTask<GameObject> InstantiateAndBind(object key, Transform parent, bool instantiateInWorldSpace)
        {
            var instance = await Instantiate(key, parent, instantiateInWorldSpace);
            instance.BindToGameObjectLifeTime().Forget();
            return instance;
        }
        
        public async UniTask<GameObject> Instantiate(object key, Transform parent, bool instantiateInWorldSpace)
        {
            var loadPair = await LoadAssetAsync(key);
            
            var component = Object.Instantiate(loadPair.asset, parent, instantiateInWorldSpace);
            return component;
        }

        public void Dispose()
        {
            
        }
    }
}