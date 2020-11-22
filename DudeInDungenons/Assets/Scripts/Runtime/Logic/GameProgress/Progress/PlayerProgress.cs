using System.Collections.Generic;

namespace Runtime.Logic.GameProgress.Progress {
    public class PlayerProgress {
        public Dictionary<ResourceId, int> Inventory = new Dictionary<ResourceId, int>();
    }
}