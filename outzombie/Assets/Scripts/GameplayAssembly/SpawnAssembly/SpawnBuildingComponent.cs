﻿using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using TriInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.SpawnAssembly
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpawnBuildingComponent : IComponent
    {
        [ReadOnly] public GameObject building;
        public List<SpawnWaveData> waves;
    }
    
    [Serializable]
    public struct SpawnWaveData
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