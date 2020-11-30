using System.Collections.Generic;

namespace Runtime.Logic.GameProgress.Progress {
    public class PlayerProgress {
        public Dictionary<string, int> Inventory = new Dictionary<string, int>();
        public string CurrentPet;
        public HashSet<string> UnlockedPets = new HashSet<string>();
        public string CurrentSkin;
        public HashSet<string> UnlockedSkins = new HashSet<string>();
    }
}