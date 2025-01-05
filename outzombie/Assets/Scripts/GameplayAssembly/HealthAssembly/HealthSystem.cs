using Scellecs.Morpeh;
using UnityEngine;

namespace GameplayAssembly.HealthSystem
{
    public class HealthSystem :  ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<HealthComponent> _healthStash;
        
        public void OnAwake()
        {
            _filter = World.Filter.With<HealthComponent>().Build();
            _healthStash = World.GetStash<HealthComponent>();
        }
        
        public void Dispose()
        {
 
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var healthComponent = ref _healthStash.Get(entity);
            }
        }
    }
}