using System;
using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress.Progress;

namespace Runtime.Logic.GameProgress {
    [Serializable]
    public class GameProgress : IProgress {
        public PlayerProgress Player = new PlayerProgress();
        public bool FirstRun = true;
        public long GameExitTime;
    }
}