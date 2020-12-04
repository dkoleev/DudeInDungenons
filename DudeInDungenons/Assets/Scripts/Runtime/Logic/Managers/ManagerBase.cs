namespace Runtime.Logic.Managers {
    public abstract class ManagerBase {
        protected readonly GameController GameController;

        protected ManagerBase(GameController gameController) {
            GameController = gameController;
        }

        public abstract void Update();
        
        public abstract void Dispose();
    }
}
