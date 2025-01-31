using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Gameplay.ObjectPoolAssembly
{
    public struct PoolObjectComponent : IComponent
    {
        public UnitsPoolContainer unitsPoolContainer;
        public string key;
        public PoolObjectProvider poolObjectProvider;

        public bool setuped;
        
        public void Setup(UnitsPoolContainer unitsPoolContainer, string key, PoolObjectProvider poolObjectProvider)
        {
            this.unitsPoolContainer = unitsPoolContainer;
            this.key = key;
            this.poolObjectProvider = poolObjectProvider;

            setuped = true;
        }

        public void Push()
        {
            unitsPoolContainer.Return(poolObjectProvider, key);

            var stash = World.Default.GetStash<DisabledComponent>();
            if (!stash.Has(poolObjectProvider.Entity))
            {
                stash.Add(poolObjectProvider.Entity, new DisabledComponent());
            }
            
            poolObjectProvider.gameObject.SetActive(false);
        }

        public void Pop(PoolObjectProvider provider)
        {
            poolObjectProvider = provider;
            var stash = World.Default.GetStash<DisabledComponent>();
            if (stash.Has(poolObjectProvider.Entity))
            {
                stash.Remove(poolObjectProvider.Entity);
            }
        }
    }
}