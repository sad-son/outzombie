using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using TriInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.SpawnAssembly
{
    [Serializable]
    public struct BuildingComponent : IComponent  
    {
       public SpawnWaveData[] waves;
    }
    
    [Serializable]
    public class SpawnWaveData
    {
        public string enemiesId;
        public float delay;
        
        public bool suspended { get; private set; }

        public void Suspend(bool value)
        {
            suspended = value;
        }
    }
}