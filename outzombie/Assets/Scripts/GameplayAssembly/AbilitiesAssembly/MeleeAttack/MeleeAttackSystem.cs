using Gameplay.EnemiesLogicAssembly;
using Gameplay.Extensions;
using Gameplay.ObjectPoolAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.AbilitiesAssembly
{
    public class MeleeAttackSystem : EcsSystem
    {
        private Filter _enemiesFilter;
        private Filter _meleeAttackTriggers;
        private Stash<MeleeAttackTriggerComponent> _triggerStash;
        private Stash<TeamComponent> _teamsStash;
        private Stash<MeleeAttackComponent> _meleeAttackStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<NavigationUnitComponent> _navigationUnitComponentStash;
        
        public override void OnAwake()
        {
            _enemiesFilter = World.Filter.With<TransformComponent>()
                .Without<DisabledComponent>().Build();
            _meleeAttackTriggers = World.Filter.With<MeleeAttackTriggerComponent>()
                .Without<DisabledComponent>().Build();
            
            _teamsStash = World.GetStash<TeamComponent>();
            _meleeAttackStash = World.GetStash<MeleeAttackComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _triggerStash = World.GetStash<MeleeAttackTriggerComponent>();
            _navigationUnitComponentStash = World.GetStash<NavigationUnitComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var meleeEntity in _meleeAttackTriggers)
            {
                ref var teamMeleeComponent = ref _teamsStash.Get(meleeEntity);
                ref var meleeTransformComponent = ref _transformStash.Get(meleeEntity);
                ref var meleeAttackComponent = ref _meleeAttackStash.Get(meleeEntity);
                ref var navigationUnitComponent = ref _navigationUnitComponentStash.Get(meleeEntity);
                
                bool hasTarget = false;
                foreach (var enemyEntity in _enemiesFilter)
                {
                    ref var teamEnemyComponent = ref _teamsStash.Get(enemyEntity);
                    ref var enemyTransform = ref _transformStash.Get(enemyEntity);
             
                    if (teamEnemyComponent.InTeam(teamMeleeComponent))
                        continue;
                    
                    var enemyPosition = enemyTransform.transform.position;
                    var meleeComponentPosition = meleeTransformComponent.transform.position;
              

                    if (Vector3.Distance(enemyPosition, meleeComponentPosition) <= meleeAttackComponent.distance)
                    {
                        if (meleeAttackComponent.canHit)
                        {
                            navigationUnitComponent.agent.updateRotation = false;
                            navigationUnitComponent.RotateToTarget(1f, enemyPosition);
                            ref var healthComponent = ref _healthStash.Get(enemyEntity);
  
                            healthComponent.Hit(meleeAttackComponent.damage);
                            meleeAttackComponent.EndAttack();
                        }
                        
                        if (meleeAttackComponent.canAttack)
                        {
                            meleeAttackComponent.StartAttack();
                        }
                        
                        hasTarget = true;
                    }
                }

                if (!hasTarget)
                {
                    navigationUnitComponent.agent.updateRotation = true;
                    _triggerStash.Remove(meleeEntity);
                }
            }
        }

    }
}