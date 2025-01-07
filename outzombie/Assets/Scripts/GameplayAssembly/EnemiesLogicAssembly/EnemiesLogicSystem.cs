using Cysharp.Threading.Tasks;
using Gameplay.SpawnAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.EnemiesLogicAssembly
{
    public class EnemiesLogicSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _buildingsFilter;
        private Filter _enemiesFilter;
        private Filter _moveToBuildingFilter;
        
        private Stash<TeamComponent> _teamStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<BuildingComponent> _buildingsStash;
        private Stash<MoveToBuildingZone> _moveToBuildingStash;
        
        public void Dispose()
        {
            
        }

        public void OnAwake()
        {
            _buildingsFilter = World.Filter.With<BuildingComponent>().Build();
            _moveToBuildingFilter = World.Filter.With<MoveToBuildingZone>().Build();
            _enemiesFilter = World.Filter.With<TeamComponent>().Without<MoveToBuildingZone>().Build();
            
            _buildingsStash = World.GetStash<BuildingComponent>();
            _moveToBuildingStash = World.GetStash<MoveToBuildingZone>();
            _teamStash = World.GetStash<TeamComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

       
        public void OnUpdate(float deltaTime)
        {
            foreach (var moveToBuildingEntity in _moveToBuildingFilter)
            {
                ref var enemyTeamComponent = ref _teamStash.Get(moveToBuildingEntity);
              
                ref var moveToBuildingComponent = ref _moveToBuildingStash.Get(moveToBuildingEntity);
                
                foreach (var building in _buildingsFilter)
                {
                    ref var buildingTeamComponent = ref _teamStash.Get(building);
                    
                    if (!enemyTeamComponent.InTeam(buildingTeamComponent))
                    {
                        ref var transformComponent = ref _transformStash.Get(building);
                        var buildingPosition = transformComponent.transform.position;
                        var destination = new Vector3(buildingPosition.x, 
                            moveToBuildingComponent.navMeshAgent.transform.position.y, 
                            buildingPosition.z);
                        
                        if (!moveToBuildingComponent.hasTarget)
                        {
                            moveToBuildingComponent.hasTarget = true;
                            moveToBuildingComponent.navMeshAgent.SetDestination(destination);
                            RotateToTarget(1f, moveToBuildingComponent.navMeshAgent.transform, destination);
                            _moveToBuildingStash.Remove(moveToBuildingEntity);
                        }

                        RotateToTarget(1f, transformComponent.transform, destination);
                        return;
                    }
                }
            }
        }
        
        private void RotateToTarget(float time, Transform transform, Vector3 destination)
        {
            var direction = destination - transform.position; 
            direction.y = 0;
            var targetRotation = Quaternion.LookRotation(direction);
            var rotation = Quaternion.Slerp(transform.rotation, targetRotation, time);
            var eulerAngles = rotation.eulerAngles;
            eulerAngles.x = -15f;

            rotation.eulerAngles = eulerAngles;
            transform.rotation = rotation;
        }
    }
}