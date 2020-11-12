using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Items {
    [CreateAssetMenu(fileName = "Item", menuName = "Data/Items/Item")]
    public class Item : ScriptableObject {
        [SerializeField]
        private string _id;
        [SerializeField]
        [HideLabel, PreviewField(55)]
        private Texture _icon;

        public string Id => _id;
    }
}