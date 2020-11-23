using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Settings {
    [CreateAssetMenu(fileName = "LevelUp", menuName = "Data/LevelUp")]
    public class LevelUp : SerializedScriptableObject {
        [SerializeField]
        private Dictionary<int, int> _levelByExp;

        public Dictionary<int, int> LevelByExp => _levelByExp; 
    }
}