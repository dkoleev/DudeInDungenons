using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress.Progress;

namespace Runtime.Logic.GameProgress {
    public class GameProgress : IProgress {
        public PlayerProgress Player = new PlayerProgress();
    }
}