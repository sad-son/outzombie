using Scellecs.Morpeh;
using Cysharp.Threading.Tasks;
using Gameplay.AbilitiesAssembly;
using Gameplay.ObjectPoolAssembly;
using Gameplay.SpawnAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.EnemiesLogicAssembly.BreakThroughToBuilding
{
    public class MoveToTargetSystem : ISystem
    {
         public World World { get; set; }
        
        private Filter _enemiesFilter;
        private Filter _meleeComponentsFilter;
        private Filter _moveToTargetFilter;
        
        private Stash<TeamComponent> _teamsStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<MoveToTargetComponent> _moveToTargetStash;
        
        public void OnAwake()
        {
            _enemiesFilter = World.Filter.With<TransformComponent>()
                .Without<DisabledComponent>().Build();
            _moveToTargetFilter = World.Filter.With<MoveToTargetComponent>()
                .Without<DisabledComponent>().Build();
            _teamsStash = World.GetStash<TeamComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _moveToTargetStash = World.GetStash<MoveToTargetComponent>();
        }
        
        public void Dispose()
        {
            
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var moveToTargetEntity in _moveToTargetFilter)
            {
                ref var moveToTargetComponent = ref _moveToTargetStash.Get(moveToTargetEntity);

                if (!moveToTargetComponent.canSelectTarget)
                    continue;
                
                ref var moveToTargetTeam = ref _teamsStash.Get(moveToTargetEntity);
                ref var transformComponent = ref _transformStash.Get(moveToTargetEntity);
        
                
                Vector3 targetPosition = Vector3.zero;
                float minDistance = 1000f;
                foreach (var enemyEntity in _enemiesFilter)
                {
                    ref var teamEnemyComponent = ref _teamsStash.Get(enemyEntity);
                    ref var enemyTransform = ref _transformStash.Get(enemyEntity);
             
                    if (teamEnemyComponent.InTeam(moveToTargetTeam))
                        continue;
                    
                    var enemyPosition = enemyTransform.transform.position;
                    var moveToTargetPosition = transformComponent.transform.position;
                    var distance = Vector3.Distance(enemyPosition, moveToTargetPosition);
                    
                   
                    if (distance <= minDistance)
                    {
                        targetPosition = enemyPosition;
                        minDistance = distance;
                    }
                }

                moveToTargetComponent.SetDestination(targetPosition);
            }
        }
    }
}