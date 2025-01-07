using System;
using Plugins.procedural_healthbar_shader.HealthBar.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.EnemiesLogicAssembly
{
    public class EnemyProvider : MonoProvider<EnemyComponent>   
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private RectTransform healthBar;
        [SerializeField] private PlayMakerFSM playMakerFsm;
        
        private void Reset()
        {
            agent = GetComponent<NavMeshAgent>();
            healthBar = GetComponentInChildren<HealthBar>().GetComponent<RectTransform>();
        }

        protected override void Initialize()
        {
            base.Initialize();
   
            agent.updateRotation = false;
            healthBar.localRotation = Quaternion.Euler(0, 180, healthBar.localRotation.eulerAngles.z);
        }

        public void AddMoveToBuildingZone()
        {
            var stash = World.Default.GetStash<MoveToBuildingZone>();

            if (!stash.Has(Entity))
            {
                stash.Add(Entity, new MoveToBuildingZone(agent));
            }
         
        }
    }
}