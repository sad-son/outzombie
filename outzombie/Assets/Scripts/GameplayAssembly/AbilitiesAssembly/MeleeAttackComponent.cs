using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.AbilitiesAssembly
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MeleeAttackComponent : IComponent
    {
        public float distance;
        public float damage;
        public float attackSpeed;

        public bool canAttack => Time.time >= _lastAttackTime + attackSpeed;
        private float _lastAttackTime;
        
        public void Attack()
        {
            _lastAttackTime = Time.time;
        }
    }
}