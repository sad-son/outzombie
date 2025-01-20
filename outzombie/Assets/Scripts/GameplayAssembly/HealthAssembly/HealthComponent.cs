using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct HealthComponent : IComponent 
{
    public float health;
    public float maxHealth;
    public GameObject gameObject;
    
    public void Hit(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
    }
}