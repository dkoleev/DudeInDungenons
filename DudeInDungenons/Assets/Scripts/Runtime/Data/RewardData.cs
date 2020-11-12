using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu]
    public class RewardData : SerializedScriptableObject {
        [SerializeField]
        private Dictionary<string, int> _reward = new Dictionary<string, int>();

        public Dictionary<string, int> Reward => _reward;
    }
}