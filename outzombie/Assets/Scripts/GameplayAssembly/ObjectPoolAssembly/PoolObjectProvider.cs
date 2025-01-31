using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Gameplay.ObjectPoolAssembly
{
    public class PoolObjectProvider : MonoProvider<PoolObjectComponent>
    {
        private UnitsPoolContainer _unitsPoolContainer;
        private string _key;
        
        public void Setup(UnitsPoolContainer unitsPoolContainer, string key)
        {
            _key = key;
            _unitsPoolContainer = unitsPoolContainer;
            ref var poolObjectComponent = ref GetData();
            poolObjectComponent.Setup(unitsPoolContainer, key, this);
            poolObjectComponent.Push();
        }
        
        public void Pop()
        {
            ref var poolObjectComponent = ref GetData();
            poolObjectComponent.Setup(_unitsPoolContainer, _key, this);
            poolObjectComponent.Pop(this);
        }
    }
}