using System;
using Plugins.procedural_healthbar_shader.HealthBar.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.EnemiesLogicAssembly
{
    public class NavigationUnitProvider : MonoProvider<NavigationUnitComponent>   
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private RectTransform healthBar;
        
        private void Reset()
        {
            agent = GetComponent<NavMeshAgent>();
            healthBar = GetComponentInChildren<HealthBar>().GetComponent<RectTransform>();
        }

        protected override void Initialize()
        {
            base.Initialize();
   
            ref var serializedData = ref GetData();
            serializedData.agent = agent;
            
          //  agent.updateRotation = false;
            healthBar.localRotation = Quaternion.Euler(0, 180, healthBar.localRotation.eulerAngles.z);
        }
    }
}