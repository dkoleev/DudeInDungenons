using System.Collections.Generic;

namespace Runtime.Logic.GameProgress.Progress {
    public class PlayerProgress {
        public Dictionary<ResourceId, int> Inventory = new Dictionary<ResourceId, int>();

        public void AddResource(ResourceId resourceId, int amount) {
            if (Inventory.ContainsKey(resourceId)) {
                Inventory[resourceId] += amount;
            } else {
                Inventory.Add(resourceId, amount);
            }
        }

        public int GetResourceAmount(ResourceId resourceId) {
            if (!Inventory.ContainsKey(resourceId)) {
                return 0;
            }

            return Inventory[resourceId];
        }
    }
}