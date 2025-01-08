using Scellecs.Morpeh;
using Cysharp.Threading.Tasks;
using Gameplay.AbilitiesAssembly;
using Gameplay.SpawnAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.EnemiesLogicAssembly.BreakThroughToBuilding
{
    public class MoveToBuildingOREnemySystem : ISystem
    {
         public World World { get; set; }
        
        private Filter _enemiesFilter;
        private Filter _meleeComponentsFilter;
        private Stash<TeamComponent> _teamsStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<TransformComponent> _transformStash;
        
        public void OnAwake()
        {
            _enemiesFilter = World.Filter.With<TeamComponent>().Build();
            _teamsStash = World.GetStash<TeamComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }
        
        public void Dispose()
        {
            
        }
        
        public void OnUpdate(float deltaTime)
        {
            /*foreach (var enemyEntity in _enemiesFilter)
            {
                ref var teamEnemyComponent = ref _teamsStash.Get(enemyEntity);
                ref var enemyTransform = ref _transformStash.Get(enemyEntity);
             
                if (teamEnemyComponent.InTeam(teamMeleeComponent))
                    continue;
                    
                var enemyPosition = enemyTransform.transform.position;
                var meleeComponentPosition = meleeTransformComponent.transform.position;
  
                if (Vector3.Distance(enemyPosition, meleeComponentPosition) <= meleeAttackComponent.distance)
                {
                    //etc
                }
            }*/
        }
    }
}