using ServiceLocatorSystem;

namespace AssetSystem
{
    public class AssetSystemLocator : SystemLocatorBase<IAssetSystemInstance>, IServiceLocator
    {

        protected override void RegisterTypes()
        {
            Register(new AssetHandler());
        }
    }
}