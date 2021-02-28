using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Runtime.Logic.CustomJsonConverters;
using Runtime.Logic.GameProgress.Progress.Items;

namespace Runtime.Logic.GameProgress.Progress {
    [Serializable]
    public class PlayerProgress {
        public Dictionary<string, ItemProgress> Inventory = new Dictionary<string, ItemProgress>();
        public string CurrentPet;
        [JsonConverter(typeof(HashSetStringConverter))]
        public HashSet<string> UnlockedPets = new HashSet<string>();
        public string CurrentSkin;
        [JsonConverter(typeof(HashSetStringConverter))]
        public HashSet<string> UnlockedSkins = new HashSet<string>();

        public ItemProgress GetInventoryItem(string key) {
            if (Inventory.ContainsKey(key)) {
                return Inventory[key];
            }

            return null;
        }
    }
}