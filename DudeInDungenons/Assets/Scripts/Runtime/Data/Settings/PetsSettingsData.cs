using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.Settings {
    [CreateAssetMenu(fileName = "PetsSettings", menuName = "Data/Settings/Pets")]
    public class PetsSettingsData : ScriptableObject{
        [SerializeField]
        private List<PetData> _pets;

        public List<PetData> Pets => _pets;
    }
}
