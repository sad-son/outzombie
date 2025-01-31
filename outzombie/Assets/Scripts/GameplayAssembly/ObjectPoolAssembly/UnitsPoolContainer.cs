using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetSystem;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay.ObjectPoolAssembly
{
    public abstract class UnitsPoolContainer : IDisposable, IGameplaySystemInstance
    {
        private readonly Dictionary<string, Queue<PoolObjectProvider>> _currentPools = new();

        private AssetHandler _assetHandler;
        private Transform _root;

        public UnitsPoolContainer(Transform root)
        {
            _root = root;
        }

        public void Dispose()
        {
            _root = null;
        }

        public bool IsEmpty()
        {
            return _currentPools.Count == 0;
        }

        public async UniTaskVoid Initialize(PoolObjectData poolObjectData)
        {
            var assetSystemLocator = ServiceLocatorController.Resolve<AssetSystemLocator>();
            _assetHandler = assetSystemLocator.Resolve<AssetHandler>();

            for (var i = 0; i < poolObjectData.size; i++)
            {
                var obj = await Instantiate(poolObjectData.key);

                if (!_currentPools.TryGetValue(poolObjectData.key, out var pool))
                {
                    pool = new Queue<PoolObjectProvider>();
                    _currentPools[poolObjectData.key] = pool;
                }

                pool.Enqueue(obj);
            }
        }

        public async UniTask<PoolObjectProvider> Get(string key)
        {
            if (!_currentPools.TryGetValue(key, out var pool))
                return default;

            if (pool.Count == 0)
            {
                var obj = await Instantiate(key);
                pool.Enqueue(obj);
            }

            var pooledObject = pool.Dequeue();
            pooledObject.gameObject.SetActive(true);
            pooledObject.Pop();
            pooledObject.transform.SetParent(_root);
            return pooledObject;
        }

        private async UniTask<PoolObjectProvider> Instantiate(string key)
        {
            var obj = await _assetHandler.InstantiateAndBind(key, _root, false);
            var poolObject = obj.AddComponent<PoolObjectProvider>();
            poolObject.Setup(this, key);
            poolObject.gameObject.SetActive(false);
            return poolObject;
        }

        public void Return(PoolObjectProvider obj, string key)
        {
            if (!_currentPools.TryGetValue(key, out var pool))
                return;

            pool.Enqueue(obj);
        }
    }
}