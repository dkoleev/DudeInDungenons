namespace Runtime.Logic.Converters {
    public interface IResourceConverter {
        void Update();
        void ApplyGameOutProgress(GameProgress.GameProgress progress);
    }
}
