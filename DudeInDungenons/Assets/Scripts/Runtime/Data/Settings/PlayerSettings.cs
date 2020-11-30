using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.Settings {
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Data/Settings/Player")]
    public class PlayerSettings : ScriptableObject {
        [SerializeField]
        private List<PlayerSkinData> _skins;

        public List<PlayerSkinData> Skins => _skins;
    }
}