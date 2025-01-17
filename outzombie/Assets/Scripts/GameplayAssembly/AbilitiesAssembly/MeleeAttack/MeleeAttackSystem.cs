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
        private Filter _meleeAttackTriggers;
        private Stash<MeleeAttackTriggerComponent> _triggerStash;
        private Stash<TeamComponent> _teamsStash;
        private Stash<MeleeAttackComponent> _meleeAttackStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<TransformComponent> _transformStash;
        
        public void OnAwake()
        {
            _enemiesFilter = World.Filter.With<TransformComponent>().Build();
            _meleeAttackTriggers = World.Filter.With<MeleeAttackTriggerComponent>().Build();
            
            _teamsStash = World.GetStash<TeamComponent>();
            _meleeAttackStash = World.GetStash<MeleeAttackComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _triggerStash = World.GetStash<MeleeAttackTriggerComponent>();
        }
        
        public void Dispose()
        {
            
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var meleeEntity in _meleeAttackTriggers)
            {
                ref var teamMeleeComponent = ref _teamsStash.Get(meleeEntity);
                ref var meleeTransformComponent = ref _transformStash.Get(meleeEntity);
                
                bool hasTarget = false;
                foreach (var enemyEntity in _enemiesFilter)
                {
                    ref var teamEnemyComponent = ref _teamsStash.Get(enemyEntity);
                    ref var enemyTransform = ref _transformStash.Get(enemyEntity);
             
                    if (teamEnemyComponent.InTeam(teamMeleeComponent))
                        continue;
                    
                    var enemyPosition = enemyTransform.transform.position;
                    var meleeComponentPosition = meleeTransformComponent.transform.position;
                    ref var meleeAttackComponent = ref _meleeAttackStash.Get(meleeEntity);
  
                    if (Vector3.Distance(enemyPosition, meleeComponentPosition) <= meleeAttackComponent.distance)
                    {
                        if (meleeAttackComponent.canAttack)
                        {
                            meleeAttackComponent.Attack();
                        }
                        
                        if (meleeAttackComponent.canHit)
                        {
                            ref var healthComponent = ref _healthStash.Get(enemyEntity);
                            healthComponent.Hit(meleeAttackComponent.damage);
                        }
                        
                        hasTarget = true;
                    }
                }

                if (!hasTarget)
                {
                    _triggerStash.Remove(meleeEntity);
                }
            }
        }

    }
}