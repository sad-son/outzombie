using System;
using System.Collections.Generic;
using AssetSystem;
using Cysharp.Threading.Tasks;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay.ObjectPoolAssembly
{
    public class EnemiesPoolContainer : IDisposable, IGameplaySystemInstance, IServiceLocator
    {
        private readonly Dictionary<string, Queue<GameObject>> _currentPools = new();
        
        private AssetHandler _assetHandler;
        private Transform _enemyRoot;
        
        public EnemiesPoolContainer(Transform enemyRoot)
        {
            _enemyRoot = enemyRoot;
        }
        
        public void Dispose()
        {
            _enemyRoot = null;
        }

        public bool IsEmpty()
        {
            return _currentPools.Count == 0;
        }
        
        public async UniTaskVoid Initialize(PoolObjectComponent poolObjectComponent)
        {
            var assetSystemLocator = ServiceLocatorController.Resolve<AssetSystemLocator>();
            _assetHandler = assetSystemLocator.Resolve<AssetHandler>();
            
            for (var i = 0; i < poolObjectComponent.size; i++)
            {
                var obj = await _assetHandler.InstantiateAndBind(poolObjectComponent.key, _enemyRoot, false);
                obj.gameObject.SetActive(false);

                if (!_currentPools.TryGetValue(poolObjectComponent.key, out var pool))
                {
                    pool = new Queue<GameObject>();
                    _currentPools[poolObjectComponent.key] = pool;
                }
                
                Debug.LogError($"Initialize {poolObjectComponent.key} _currentPools.Count {_currentPools.Count}");
                pool.Enqueue(obj);
            }
        }

        public async UniTask<GameObject> Get(string key) 
        {
            Debug.LogError($"Get {key} _currentPools.Count {_currentPools.Count}");

            foreach (var kvp in _currentPools)
            {
                Debug.LogError($"Get _currentPools  {kvp.Key} {kvp.Value.Count}");
            }
            if (!_currentPools.TryGetValue(key, out var pool))
                return default;
            
            if (pool.Count == 0)
            {
                var obj = await _assetHandler.InstantiateAndBind(key, _enemyRoot, false);
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }

            var pooledObject = pool.Dequeue();
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public void Return(GameObject obj, string key) 
        {
            if (!_currentPools.TryGetValue(key, out var pool))
                return;
            
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}