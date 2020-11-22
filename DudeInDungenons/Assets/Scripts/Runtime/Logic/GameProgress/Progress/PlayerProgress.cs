using System.Collections.Generic;

namespace Runtime.Logic.GameProgress.Progress {
    public class PlayerProgress {
        public Dictionary<ResourceId, int> Inventory = new Dictionary<ResourceId, int>();

        public int GetResourceAmount(ResourceId id) {
            if (!Inventory.ContainsKey(id)) {
                return 0;
            }

            return Inventory[id];
        }
    }
}