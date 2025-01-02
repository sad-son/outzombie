using System;
using Gameplay.ObjectPoolAssembly;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay
{
    public class GameplayLoader : MonoBehaviour
    {
        [SerializeField] private Transform _enemyRoot;
        [SerializeField] private PoolObjectComponent[] _enemyPoolObjects;
        
        private void Awake()
        {
            ServiceLocatorController.Register(new GameplaySystemLocator(_enemyRoot));

            foreach (var poolObjectComponent in _enemyPoolObjects)
            {
                ServiceLocatorController.Resolve<GameplaySystemLocator>()
                    .Resolve<EnemiesPoolContainer>()
                    .Initialize(poolObjectComponent).Forget();
            }
        }

        private void OnDestroy()
        {
            ServiceLocatorController.Unregister<GameplaySystemLocator>();
        }
    }
}