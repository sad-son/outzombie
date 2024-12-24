using AssetSystem;
using Cysharp.Threading.Tasks;
using WindowsSystem.Concrete.Cheats;

namespace WindowsSystem.Concrete
{
    public class GameplayHUD : WindowBase
    {
        public void ShowCheats()
        {
            var windowsController = AssetSystemLocatorController.Resolve<WindowsController>();
            windowsController.Show<CheatsWindow>(WindowLayer.Technical).Forget();
        }
    }
}