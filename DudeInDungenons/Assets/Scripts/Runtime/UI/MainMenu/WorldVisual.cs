using Runtime.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.UI.MainMenu {
    public class WorldVisual : MonoBehaviour {
        [SerializeField,Required]
        private WorldData _data;

        public WorldData Data => _data;
    }
}