using Avocado.Framework.Patterns.Singleton;
using Input;

namespace Avocado.Input {
    public class InputManager : Singleton<InputManager> {
        public Controls MainControl { get; }

        public InputManager() {
            MainControl = new Controls();
        }
    }
}