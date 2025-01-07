using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.UnityComponents
{
    public struct TransformComponent : IComponent
    {
        [NonSerialized] public Transform transform;
    }
}