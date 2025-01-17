using System;
using Gameplay.ObjectPoolAssembly;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay
{
    public class GameplayLoader : MonoBehaviour
    {
        [SerializeField] private Transform _enemyRoot;
        [SerializeField] private Transform _myRoot;
        [SerializeField] private PoolObjectComponent[] _enemyPoolObjects;
        [SerializeField] private PoolObjectComponent[] _myUnitsObjects;
        
        private void Awake()
        {
            ServiceLocatorController.Register(new GameplaySystemLocator(_enemyRoot, _myRoot));

            foreach (var poolObjectComponent in _enemyPoolObjects)
            {
                ServiceLocatorController.Resolve<GameplaySystemLocator>()
                    .Resolve<EnemiesPoolContainer>()
                    .Initialize(poolObjectComponent).Forget();
            }
            
            foreach (var poolObjectComponent in _myUnitsObjects)
            {
                ServiceLocatorController.Resolve<GameplaySystemLocator>()
                    .Resolve<MyUnitsPoolContainer>()
                    .Initialize(poolObjectComponent).Forget();
            }
        }

        private void OnDestroy()
        {
            ServiceLocatorController.Unregister<GameplaySystemLocator>();
        }
    }
}