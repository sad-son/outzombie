using ServiceLocatorSystem;

namespace Systems
{
    public class InputSystemLocator : SystemLocatorBase<IInputSystemInstance>
    {
        protected override void RegisterTypes()
        {
            Register(new InputManager());
        }
    }
}