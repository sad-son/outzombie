using System;
using System.Collections.Generic;
using AssetSystem;
using Cysharp.Threading.Tasks;
using ServiceLocatorSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor.VersionControl;
using UnityEngine;

namespace WindowsSystem
{
    public enum WindowLayer
    {
        Foreground,
        Technical
    }
    
    public class WindowsController : SerializedMonoBehaviour, IWindowsController, IAssetSystemInstance, IServiceLocator
    {
        [OdinSerialize] public Dictionary<WindowLayer, RectTransform> _layers = new();

        private readonly Dictionary<Type, IWindow> _currentWindows = new();
        
        private AssetHandler _assetHandler;

        private void Awake()
        {
            var assetSystemLocator = ServiceLocatorController.Resolve<AssetSystemLocator>();
            _assetHandler = assetSystemLocator.Resolve<AssetHandler>();
        }

        public async UniTask<T> Show<T>(WindowLayer layer) where T : Component, IWindow
        {
            var type = typeof(T);

            if (_currentWindows.TryGetValue(type, out var currentWindow) 
                && currentWindow is T existWindow)
            {
                existWindow.Show();
                return existWindow;
            }

            var parentLayer = _layers[layer];
            var window = await _assetHandler.InstantiateAndBind<T>(type.Name, parentLayer, false);
            window.Show();
            _currentWindows[type] = window;
            return window;
        }

        public void Register()
        {
            
        }
    }
}