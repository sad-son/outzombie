using System;
using Gameplay.AbilitiesAssembly;
using Gameplay.EnemiesLogicAssembly;
using Gameplay.EnemiesLogicAssembly.BreakThroughToBuilding;
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
            systemGroup.AddSystem(new SpawnBuildingSystem());
            systemGroup.AddSystem(new SpawnCardSystem());
            systemGroup.AddSystem(new MovingToBuildingSystem());
            systemGroup.AddSystem(new MoveToTargetSystem());
            systemGroup.AddSystem(new MeleeAttackSystem());
            
            world.AddSystemsGroup(order: 0, systemGroup);
        }
    }
}