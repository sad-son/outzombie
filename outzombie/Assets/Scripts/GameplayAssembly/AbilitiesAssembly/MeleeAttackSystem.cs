using Gameplay.EnemiesLogicAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.AbilitiesAssembly
{
    public class MeleeAttackSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _enemiesFilter;
        private Filter _meleeComponentsFilter;
        private Stash<TeamComponent> _teamsStash;
        private Stash<MeleeAttackComponent> _meleeAttackStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<TransformComponent> _transformStash;
        
        public void OnAwake()
        {
            _enemiesFilter = World.Filter.With<TeamComponent>().Build();
            _meleeComponentsFilter = World.Filter.With<MeleeAttackComponent>().Build();
            _teamsStash = World.GetStash<TeamComponent>();
            _meleeAttackStash = World.GetStash<MeleeAttackComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }
        
        public void Dispose()
        {
            
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var meleeEntity in _meleeComponentsFilter)
            {
                ref var teamMeleeComponent = ref _teamsStash.Get(meleeEntity);
                ref var meleeTransformComponent = ref _transformStash.Get(meleeEntity);
                
                foreach (var enemyEntity in _enemiesFilter)
                {
                    ref var teamEnemyComponent = ref _teamsStash.Get(enemyEntity);
                    ref var enemyTransform = ref _transformStash.Get(enemyEntity);
             
                    if (teamEnemyComponent.InTeam(teamMeleeComponent))
                        continue;
                    
                    var enemyPosition = enemyTransform.transform.position;
                    var meleeComponentPosition = meleeTransformComponent.transform.position;
                    ref var meleeAttackComponent = ref _meleeAttackStash.Get(meleeEntity);
  
                    if (meleeAttackComponent.canAttack
                        && Vector3.Distance(enemyPosition, meleeComponentPosition) <= meleeAttackComponent.distance)
                    {
                        ref var healthComponent = ref _healthStash.Get(enemyEntity);
                        healthComponent.Hit(meleeAttackComponent.damage);
                        
                        meleeAttackComponent.Attack();
                    }
                }
            }
        }

    }
}