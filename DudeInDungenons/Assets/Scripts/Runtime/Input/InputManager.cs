using Avocado.Framework.Patterns.Singleton;

namespace Runtime.Input {
    public class InputManager : Singleton<InputManager> {
        public Controls MainControl { get; }

        public InputManager() {
            MainControl = new Controls();
        }
    }
}