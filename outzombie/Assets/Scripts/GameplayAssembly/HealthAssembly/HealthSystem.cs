using Gameplay.EnemiesLogicAssembly;
using Gameplay.Extensions;
using Gameplay.ObjectPoolAssembly;
using Gameplay.SpawnAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace GameplayAssembly.HealthSystem
{
    public class HealthSystem :  EcsSystem
    {
        private Filter _filter;
        private Filter _buildingFilter;
        private Stash<HealthComponent> _healthStash;
        private Stash<BuildingComponent> _buildingStash;
        private Stash<PoolObjectComponent> _poolObjectStash;
        private Stash<TransformComponent> _transformStash;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<HealthComponent>().Without<DisabledComponent>().Build();
            _buildingFilter = World.Filter.With<BuildingComponent>().Without<DisabledComponent>().Build();
            
            _buildingStash = World.GetStash<BuildingComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _poolObjectStash = World.GetStash<PoolObjectComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var healthComponent = ref _healthStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
           
                if (healthComponent.health <= 0)
                {
                    if (_poolObjectStash.Has(entity))
                    {
                        ref var poolObjectComponent = ref _poolObjectStash.Get(entity);
                        poolObjectComponent.Push();
                    }
                    else
                    {
                        Object.Destroy(transformComponent.transform.gameObject);

                        if (_buildingStash.Has(entity))
                        {
                            ref var buildingComponent = ref _buildingStash.Get(entity);

                            if (buildingComponent.isMain)
                            {
                                
                            }
                        }
                    }
                }
            }
        }
    }
}