using Gameplay.ObjectPoolAssembly;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay
{
    public class GameplaySystemLocator : SystemLocatorBase<IGameplaySystemInstance>
    {
        private readonly Transform _enemyRoot;
        private readonly Transform _myRoot;

        public GameplaySystemLocator(Transform enemyRoot, Transform myRoot)
        {
            _enemyRoot = enemyRoot;
            _myRoot = myRoot;
        }

        protected override void RegisterTypes()
        {
            Register(new EnemiesPoolContainer(_enemyRoot));
            Register(new MyUnitsPoolContainer(_myRoot));
        }
    }
}