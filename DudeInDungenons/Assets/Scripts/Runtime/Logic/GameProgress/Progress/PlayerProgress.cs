using System.Collections.Generic;

namespace Runtime.Logic.GameProgress.Progress {
    public class PlayerProgress {
        public Dictionary<ResourceId, int> Inventory = new Dictionary<ResourceId, int>();
        public string CurrentPet;
        public HashSet<string> UnlockedPets = new HashSet<string>();
    }
}