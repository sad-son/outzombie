using Gameplay.ObjectPoolAssembly;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay
{
    public class GameplaySystemLocator : SystemLocatorBase<IGameplaySystemInstance>, IServiceLocator
    {
        private readonly Transform _enemyRoot;

        public GameplaySystemLocator(Transform enemyRoot)
        {
            _enemyRoot = enemyRoot;
        }

        protected override void RegisterTypes()
        {
            Register(new EnemiesPoolContainer(_enemyRoot));
        }
    }
}