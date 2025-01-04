using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.EnemiesLogicAssembly
{
    public class EnemyProvider : MonoProvider<EnemyComponent>   
    {
        [SerializeField] private NavMeshAgent agent;

        protected override void Initialize()
        {
            base.Initialize();
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        public void AddMoveToBuildingZone()
        {
            var healthStash = World.Default.GetStash<MoveToBuildingZone>();
            if (!healthStash.Has(Entity))
            {
                healthStash.Add(Entity, new MoveToBuildingZone(agent));
                Debug.Log("AddMoveToBuildingZone");
            }
         
        }
    }
}