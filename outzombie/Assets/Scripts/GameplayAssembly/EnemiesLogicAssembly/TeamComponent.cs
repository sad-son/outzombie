using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.EnemiesLogicAssembly
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TeamComponent : IComponent
    {
        public byte team;

        public override bool Equals(object obj)
        {
            if (obj is TeamComponent teamComponent) return team == teamComponent.team;
            return false;
        }
    }
    
    public static class TeamExtensions 
    {
        public static bool InTeam(this TeamComponent first, TeamComponent second) 
        {
            return first.team.Equals(second.team);
        }
    }
}