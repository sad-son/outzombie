using ServiceLocatorSystem;

namespace AssetSystem
{
    public class AssetSystemLocator : SystemLocatorBase<IAssetSystemInstance>
    {

        protected override void RegisterTypes()
        {
            Register(new AssetHandler());
        }
    }
}