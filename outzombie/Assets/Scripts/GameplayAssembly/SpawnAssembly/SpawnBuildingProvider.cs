using Scellecs.Morpeh.Providers;

namespace Gameplay.SpawnAssembly
{
    public class SpawnBuildingProvider : MonoProvider<SpawnBuildingComponent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            ref var data = ref GetData();
            data.building = this.gameObject;
        }
    }
}