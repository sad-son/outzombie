using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WindowsSystem
{
    public interface IWindowsController
    {
        UniTask<T> Show<T>(WindowLayer layer) where T : Component, IWindow;
    }
}