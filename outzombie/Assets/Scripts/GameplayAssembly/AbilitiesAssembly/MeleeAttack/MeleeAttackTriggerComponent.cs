using System;
using Scellecs.Morpeh;

namespace Gameplay.AbilitiesAssembly
{
    public struct MeleeAttackTriggerComponent : IComponent, IDisposable
    {
        public Action onDestroyed;

        public MeleeAttackTriggerComponent(Action onDestroyed)
        {
            this.onDestroyed = onDestroyed;
        }

        public void Dispose()
        {
            onDestroyed?.Invoke();
            onDestroyed = null;
        }
    }
}