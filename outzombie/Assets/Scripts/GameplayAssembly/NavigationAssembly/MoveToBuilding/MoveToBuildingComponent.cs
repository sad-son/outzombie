using System;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.EnemiesLogicAssembly
{
    public struct MoveToBuildingComponent : IComponent, IDisposable
    {
        public NavMeshAgent navMeshAgent;
        public bool hasTarget;

        public Action disposed;
        
        public MoveToBuildingComponent(NavMeshAgent navMeshAgent)
        {
            this.navMeshAgent = navMeshAgent;
            hasTarget = false;
            disposed = null;
        }

        public MoveToBuildingComponent(NavMeshAgent navMeshAgent, Action onDestroy)
        {
            this.navMeshAgent = navMeshAgent;
            hasTarget = false;
            disposed = onDestroy;
        }
        
        public void Dispose()
        {
            disposed?.Invoke();
            disposed = null;
            hasTarget = false;
        }
    }
}