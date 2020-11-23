using Runtime.Data.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class SettingsReference : MonoBehaviour {
        [SerializeField, Required]
        private LevelUp _levelUp;

        public LevelUp LevelUp => _levelUp;
    }
}