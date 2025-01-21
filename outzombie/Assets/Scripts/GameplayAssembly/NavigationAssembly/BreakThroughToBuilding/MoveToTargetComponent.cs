using System;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.EnemiesLogicAssembly.BreakThroughToBuilding
{
    public struct MoveToTargetComponent : IComponent, IDisposable
    {
        public NavMeshAgent navMeshAgent;
        public float findTargetDelay;
        public Action completed;

        public MoveToTargetComponent(NavMeshAgent navMeshAgent, Action completed) : this()
        {
            this.navMeshAgent = navMeshAgent;
            this.completed = completed;
            findTargetDelay = 1;
        }

        public bool hasTarget { get; private set; }
        public bool canSelectTarget => Time.time >= _lastTargetSelecting + findTargetDelay;

        private float _lastTargetSelecting;

        public void SetDestination(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            _lastTargetSelecting = Time.time;
            
            if (!navMeshAgent.pathPending 
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                Complete();
            }
            
            /*if (!navMeshAgent.pathPending 
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                && navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                Complete();
            }*/
        }

        public void Complete()
        {
            completed?.Invoke();
        }
        public void Dispose()
        {
            completed?.Invoke();
            completed = null;
            hasTarget = false;
        }
    }
}