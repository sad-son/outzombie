using Scellecs.Morpeh.Providers;

namespace Gameplay.UnityComponents
{
    public class TransformProvider : MonoProvider<TransformComponent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            ref var data = ref GetData();
            data.transform = transform;
        }
    }
}