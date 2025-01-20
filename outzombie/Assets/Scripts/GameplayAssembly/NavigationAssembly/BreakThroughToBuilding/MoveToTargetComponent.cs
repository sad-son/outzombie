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
        public Action disposed;

        public MoveToTargetComponent(NavMeshAgent navMeshAgent, Action disposed) : this()
        {
            this.navMeshAgent = navMeshAgent;
            this.disposed = disposed;
            findTargetDelay = 1;
        }

        public bool hasTarget { get; private set; }
        public bool canSelectTarget => Time.time >= _lastTargetSelecting + findTargetDelay;

        private float _lastTargetSelecting;

        public void SetDestination(Vector3 destination)
        {
            Debug.LogError($"SAD {navMeshAgent} to target destination {destination}");
            navMeshAgent.SetDestination(destination);
            _lastTargetSelecting = Time.time;
        }

        public void Dispose()
        {
            disposed?.Invoke();
            disposed = null;
            hasTarget = false;
        }
    }
}