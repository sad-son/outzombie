using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.CardAssembly
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CardUiComponent : IComponent
    {
        public int cost;
        public string unitId;
        public bool readyToSpawn;
        public Vector3 position;
    }
}