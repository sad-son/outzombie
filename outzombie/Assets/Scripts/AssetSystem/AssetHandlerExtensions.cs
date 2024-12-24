using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetSystem
{
    public static class AssetHandlerExtensions
    {
        public static async UniTaskVoid BindToGameObjectLifeTime(this GameObject component)
        {
            await component.OnDestroyAsync();
            Addressables.ReleaseInstance(component.gameObject);
        } 
    }
}