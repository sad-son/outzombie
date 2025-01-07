using System;
using Cysharp.Threading.Tasks;
using Gameplay.EnemiesLogicAssembly;
using Gameplay.ObjectPoolAssembly;
using Gameplay.UnityComponents;
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
        private Stash<TeamComponent> _teamStash;
        private Stash<TransformComponent> _transformStash;
        private EnemiesPoolContainer _enemiesPoolContainer;
        
        public void Dispose()
        {
            
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<BuildingComponent>().Build();
            _stash = World.GetStash<BuildingComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _transformStash = World.GetStash<TransformComponent>();
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
                ref var teamComponent = ref _teamStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                var spawnPosition = transformComponent.transform.position + new Vector3(0, 0, -2);
                
                foreach (var wave in spawnBuildingComponent.waves)
                {
                    if (!wave.suspended)
                        Spawn(wave, spawnPosition, teamComponent.team).Forget();
                }
            }
        }

        private async UniTaskVoid Spawn(SpawnWaveData wave, Vector3 position, byte team)
        {
            wave.Suspend(true);
            var enemy = await _enemiesPoolContainer.Get(wave.enemiesId);
            enemy.transform.position = position;
            enemy.transform.rotation = Quaternion.Euler(0, -180, 0);
            if (enemy.TryGetComponent(out TeamProvider teamProvider))
            {
                teamProvider.SetTeam(team);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(wave.delay));
            wave.Suspend(false);
        }
    }
}