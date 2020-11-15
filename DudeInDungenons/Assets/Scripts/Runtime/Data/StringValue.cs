using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "StringItem", menuName = "Data/Base/StringItem")]
    public class StringValue : ScriptableObject {
        [SerializeField]
        private string _value;

        public string Value => _value;
    }
}