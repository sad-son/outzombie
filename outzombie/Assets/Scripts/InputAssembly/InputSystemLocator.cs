using ServiceLocatorSystem;

namespace Systems
{
    public class InputSystemLocator : SystemLocatorBase<IInputSystemInstance>, IServiceLocator
    {
        protected override void RegisterTypes()
        {
            Register(new InputManager());
        }
    }
}