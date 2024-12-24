using AssetSystem;
using Cysharp.Threading.Tasks;
using SceneSystem;
using ServiceLocatorSystem;
using UnityEngine;
using WindowsSystem;
using WindowsSystem.Concrete;

namespace BootSystem
{
    public class BootLoader : MonoBehaviour
    {
        private TimeMeasure _timeMeasure;
    
        private async void Start()
        {
            _timeMeasure = new TimeMeasure(this);
            _timeMeasure.CommitStartTime();
            SceneController.LoadScene(SceneType.Gameplay);
            ServiceLocatorController.Register(() => new AssetSystemLocator());
            await InitializeWindowsSystem();
            _timeMeasure.CommitEndTime();
        }

        private static async UniTask InitializeWindowsSystem()
        {
            await AssetSystemLocatorController.Register<WindowsController>(nameof(WindowsController), null);
            var windowsController = AssetSystemLocatorController.Resolve<WindowsController>();
            DontDestroyOnLoad(windowsController);
            
            windowsController.Show<GameplayHUD>(WindowLayer.Foreground).Forget();
        }
    }
}
