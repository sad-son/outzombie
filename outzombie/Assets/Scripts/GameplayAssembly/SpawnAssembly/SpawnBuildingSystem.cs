using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.EnemiesLogicAssembly;
using Gameplay.Extensions;
using Gameplay.ObjectPoolAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay.SpawnAssembly
{
    public class SpawnBuildingSystem  :  EcsSystem
    {
        private Filter _filter;
        private Stash<BuildingComponent> _stash;
        private Stash<TeamComponent> _teamStash;
        private Stash<TransformComponent> _transformStash;
        private EnemiesPoolContainer _enemiesPoolContainer;
        
        private bool _spawning;
        private CancellationTokenSource _cancellationTokenSource;
        
        public override void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        public override void OnAwake()
        {
            _filter = World.Filter.With<BuildingComponent>()
                .Without<DisabledComponent>()
                .Build();
            _stash = World.GetStash<BuildingComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _enemiesPoolContainer = ServiceLocatorController.Resolve<GameplaySystemLocator>()
                .Resolve<EnemiesPoolContainer>();
            
            _cancellationTokenSource =  new CancellationTokenSource();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            if (_spawning)
                return;
            
            if (_enemiesPoolContainer.IsEmpty())
                return;
            
            foreach (var entity in _filter)
            {
                ref var spawnBuildingComponent = ref _stash.Get(entity);
                ref var teamComponent = ref _teamStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                var spawnPosition = transformComponent.transform.position + new Vector3(0, 0, -2);
                
                SpawnWaves(spawnBuildingComponent, spawnPosition, teamComponent).Forget();
            }
        }

        private async UniTaskVoid SpawnWaves(BuildingComponent spawnBuildingComponent, Vector3 spawnPosition, TeamComponent teamComponent)
        {
            _spawning = true;
            foreach (var wave in spawnBuildingComponent.waves)
            {
                if (!wave.suspended)
                    Spawn(wave, spawnPosition, teamComponent.team).Forget();
                
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _cancellationTokenSource.Token);
            }
            _spawning = false;
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
            await UniTask.Delay(TimeSpan.FromSeconds(wave.delay), cancellationToken: _cancellationTokenSource.Token);
            wave.Suspend(false);
        }
    }
}