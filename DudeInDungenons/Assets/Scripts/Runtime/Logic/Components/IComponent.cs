namespace Runtime.Logic.Components {
    public interface IComponent {
        //Call this method in "Start" method of Entity.
        void Initialize();
    }
}