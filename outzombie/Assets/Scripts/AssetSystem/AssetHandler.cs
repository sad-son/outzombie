using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetSystem
{
    public class AssetHandler : IAssetSystemInstance
    {
        public async UniTask<(AsyncOperationHandle<GameObject> asyncOperationHandle, GameObject asset)> LoadAssetAsync(object key)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(key);
            var asset = await asyncOperationHandle.Task.AsUniTask();
            return (asyncOperationHandle, asset);
        }

        public async UniTask<T> InstantiateAndBind<T>(object prefab, Transform parent, bool instantiateInWorldSpace) where T : Object
        {
            var instance = await Instantiate(prefab, parent, instantiateInWorldSpace);
            instance.BindToGameObjectLifeTime().Forget();
            return instance.GetComponent<T>();
        }

        public async UniTask<GameObject> Instantiate(object prefab, Transform parent, bool instantiateInWorldSpace)
        {
            var loadPair = await LoadAssetAsync(prefab);
            
            var component = Object.Instantiate(loadPair.asset, parent, instantiateInWorldSpace);
            return component;
        }
    }
}