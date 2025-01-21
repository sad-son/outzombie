using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.EnemiesLogicAssembly
{
    public struct NavigationUnitComponent : IComponent
    {
        public NavMeshAgent agent;
        
        public void RotateToTarget(float time, Vector3 destination)
        {
            var transform = agent.transform;
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