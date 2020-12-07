using System;
using System.Collections.Generic;
using Runtime.Logic.GameProgress.Progress.Items;

namespace Runtime.Logic.GameProgress.Progress {
    [Serializable]
    public class PlayerProgress {
        public Dictionary<string, ItemProgress> Inventory = new Dictionary<string, ItemProgress>();
        public string CurrentPet;
        public HashSet<string> UnlockedPets = new HashSet<string>();
        public string CurrentSkin;
        public HashSet<string> UnlockedSkins = new HashSet<string>();
    }
}