using Gameplay.EnemiesLogicAssembly;
using Gameplay.ObjectPoolAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace GameplayAssembly.HealthSystem
{
    public class HealthSystem :  ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<HealthComponent> _healthStash;
        private Stash<PoolObjectComponent> _poolObjectStash;
        private Stash<TransformComponent> _transformStash;
        
        public void OnAwake()
        {
            _filter = World.Filter.With<HealthComponent>()
                .Without<DisabledComponent>().Build();
            _healthStash = World.GetStash<HealthComponent>();
            _poolObjectStash = World.GetStash<PoolObjectComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }
        
        public void Dispose()
        {
 
        }
        
        public void OnUpdate(float deltaTime)
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
                        //Debug.LogError($"SAD TRY DESTROY {entity}");
                        //Object.Destroy(transformComponent.transform.gameObject);
                    }
                }
            }
        }
    }
}