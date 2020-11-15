using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "World", menuName = "Data/World")]
    public class WorldData : ScriptableObject {
        [SerializeField, Required]
        [InlineEditor]
        private List<StringValue> _levels;

        public List<StringValue> Levels => _levels;
    }
}