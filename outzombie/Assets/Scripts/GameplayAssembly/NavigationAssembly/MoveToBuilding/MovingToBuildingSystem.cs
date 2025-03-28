﻿using Cysharp.Threading.Tasks;
using Gameplay.Extensions;
using Gameplay.ObjectPoolAssembly;
using Gameplay.SpawnAssembly;
using Gameplay.UnityComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.EnemiesLogicAssembly
{
    public class MovingToBuildingSystem : EcsSystem
    {
        private Filter _buildingsFilter;
        private Filter _enemiesFilter;
        private Filter _moveToBuildingFilter;
        
        private Stash<TeamComponent> _teamStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<BuildingComponent> _buildingsStash;
        private Stash<MoveToBuildingComponent> _moveToBuildingStash;

        public override void OnAwake()
        {
            _buildingsFilter = World.Filter.With<BuildingComponent>()
                .Without<DisabledComponent>().Build();
            _moveToBuildingFilter = World.Filter.With<MoveToBuildingComponent>()
                .Without<DisabledComponent>().Build();
            _enemiesFilter = World.Filter.With<TeamComponent>().Without<MoveToBuildingComponent>().Build();
            
            _buildingsStash = World.GetStash<BuildingComponent>();
            _moveToBuildingStash = World.GetStash<MoveToBuildingComponent>().AsDisposable();
            _teamStash = World.GetStash<TeamComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

       
        public override void OnUpdate(float deltaTime)
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
        
                        var agent = moveToBuildingComponent.navMeshAgent;
                        
                        if (!moveToBuildingComponent.hasTarget)
                        {
                            moveToBuildingComponent.hasTarget = true;
                            agent.SetDestination(destination);
                        }

                        if (!agent.pathPending 
                            && agent.remainingDistance <= agent.stoppingDistance
                            && agent.velocity.sqrMagnitude == 0f)
                        {
                            _moveToBuildingStash.Remove(moveToBuildingEntity);
                        }
                    }
                }
            }
        }

    }
}