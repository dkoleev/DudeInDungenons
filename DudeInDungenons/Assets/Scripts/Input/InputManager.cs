using Avocado.Framework.Patterns.Singleton;

namespace Input {
    public class InputManager : Singleton<InputManager> {
        public Controls MainControl { get; }

        public InputManager() {
            MainControl = new Controls();
        }
    }
}