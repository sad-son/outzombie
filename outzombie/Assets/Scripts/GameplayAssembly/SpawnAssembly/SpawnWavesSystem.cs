using System;
using Cysharp.Threading.Tasks;
using Gameplay.ObjectPoolAssembly;
using Scellecs.Morpeh;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay.SpawnAssembly
{
    public class SpawnWavesSystem  :  ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<BuildingComponent> _stash;
        private EnemiesPoolContainer _enemiesPoolContainer;
        
        public void Dispose()
        {
            
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<BuildingComponent>().Build();
            _stash = World.GetStash<BuildingComponent>();
            _enemiesPoolContainer = ServiceLocatorController.Resolve<GameplaySystemLocator>()
                .Resolve<EnemiesPoolContainer>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_enemiesPoolContainer.IsEmpty())
                return;
            foreach (var entity in _filter)
            {
                ref var spawnBuildingComponent = ref _stash.Get(entity);
                var spawnPosition = spawnBuildingComponent.building.transform.position + new Vector3(0, 0, -2);
                
                foreach (var wave in spawnBuildingComponent.waves)
                {
                    if (!wave.suspended)
                        Spawn(wave, spawnPosition).Forget();
                }
            }
        }

        private async UniTaskVoid Spawn(SpawnWaveData wave, Vector3 position)
        {
            wave.Suspend(true);
            var enemy = await _enemiesPoolContainer.Get(wave.enemiesId);
            enemy.transform.position = position;
            await UniTask.Delay(TimeSpan.FromSeconds(wave.delay));
            wave.Suspend(false);
        }
    }
}