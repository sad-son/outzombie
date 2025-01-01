using UnityEngine;

namespace WindowsSystem.Concrete
{
    public abstract class WindowBase : MonoBehaviour, IWindow
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}