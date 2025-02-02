using Scellecs.Morpeh;

namespace Gameplay.Extensions
{
    public abstract class EcsSystem : ISystem
    {
        public World World { get; set; }
        
        public virtual void OnAwake()
        {
            
            
        }
        public virtual void Dispose()
        {
            
        }
        
        public virtual void OnUpdate(float deltaTime)
        {
            
        }
    }
}