using Scellecs.Morpeh;
using UnityEngine.AI;

namespace Gameplay.EnemiesLogicAssembly
{
    public struct MoveToBuildingZone : IComponent
    {
        public NavMeshAgent navMeshAgent;

        public MoveToBuildingZone(NavMeshAgent navMeshAgent)
        {
            this.navMeshAgent = navMeshAgent;
        }
    }
}