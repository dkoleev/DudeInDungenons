using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu]
    public class Item : ScriptableObject {
        [SerializeField]
        private string _id;
        [SerializeField]
        [HideLabel, PreviewField(55)]
        private Texture _icon;

        public string Id => _id;
    }
}