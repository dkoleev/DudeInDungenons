using Runtime.Data.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class SettingsReference : MonoBehaviour {
        [SerializeField, Required]
        private LevelUp _levelUp;
        [SerializeField, Required]
        private PetsSettingsData _pets;
        [SerializeField, Required]
        private PlayerSettings _player;

        public LevelUp LevelUp => _levelUp;
        public PetsSettingsData Pets => _pets;
        public PlayerSettings Player => _player;
    }
}