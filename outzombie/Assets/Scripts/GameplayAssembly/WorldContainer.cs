using System;
using Gameplay.AbilitiesAssembly;
using Gameplay.EnemiesLogicAssembly;
using Gameplay.SpawnAssembly;
using GameplayAssembly.HealthSystem;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay
{
    public class WorldContainer : MonoBehaviour
    {
        private void Start()
        {
            var world = World.Default;

            var systemGroup = world.CreateSystemsGroup();
            systemGroup.AddSystem(new HealthSystem());
            systemGroup.AddSystem(new SpawnWavesSystem());
            systemGroup.AddSystem(new EnemiesLogicSystem());
            systemGroup.AddSystem(new MeleeAttackSystem());
            
            world.AddSystemsGroup(order: 0, systemGroup);
        }
    }
}