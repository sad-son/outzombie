using System;
using Plugins.procedural_healthbar_shader.HealthBar.Components;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace GameplayAssembly.HealthSystem
{
    public class HealthProvider : MonoProvider<HealthComponent>   
    {
        public float healthNormalized
        {
            get
            {
                ref var serializedData = ref GetData();
                return serializedData.health / serializedData.maxHealth;
            }
        }
    }
}