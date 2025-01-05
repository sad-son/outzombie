using Gameplay.SpawnAssembly;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.EnemiesLogicAssembly
{
    public class EnemiesLogicSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _buildingsFilter;
        private Filter _moveToBuildingFilter;
        
        private Stash<BuildingComponent> _buildingsStash;
        private Stash<MoveToBuildingZone> _moveToBuildingStash;
        
        public void Dispose()
        {
            
        }

        public void OnAwake()
        {
            _buildingsFilter = World.Filter.With<BuildingComponent>().Build();
            _moveToBuildingFilter = World.Filter.With<MoveToBuildingZone>().Build();
            
            _buildingsStash = World.GetStash<BuildingComponent>();
            _moveToBuildingStash = World.GetStash<MoveToBuildingZone>();
        }

       
        public void OnUpdate(float deltaTime)
        {
            foreach (var moveToBuildingEntity in _moveToBuildingFilter)
            {
                ref var moveToBuildingComponent = ref _moveToBuildingStash.Get(moveToBuildingEntity);
                
                foreach (var building in _buildingsFilter)
                {
                    ref var buildingComponent = ref _buildingsStash.Get(building);

                    if (buildingComponent.team == Teams.TeamConstants.myTeam)
                    {
                        var buildingPosition = buildingComponent.building.transform.position;
                        var destination = new Vector3(buildingPosition.x, 
                            moveToBuildingComponent.navMeshAgent.transform.position.y, 
                            buildingPosition.z);

                        if (destination != moveToBuildingComponent.navMeshAgent.destination)
                        {
                            moveToBuildingComponent.navMeshAgent.SetDestination(destination);
   
                            _moveToBuildingStash.Remove(moveToBuildingEntity);
                        }
                        return;
                    }
                }
            }
           
        }
    }
}