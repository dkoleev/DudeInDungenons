using Runtime.Data;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;
using Runtime.Utilities;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Equipment {
    public class PetShopItem : MonoBehaviour {
        [SerializeField, Required]
        private Button _button;
        [SerializeField, Required]
        private Image _icon;
        [SerializeField, Required]
        private Image _defaultBack;
        [SerializeField, Required]
        private Image _selectedBack;
        
        public Relay<PetShopItem> OnSelected = new Relay<PetShopItem>();

        private PetData _petData;

        private void Awake() {
            _button.onClick.AddListener(SelectPet);
        }

        public void SetContent(PetData petData, bool selected) {
            _petData = petData;
            
            SetSelected(selected);
            
            _icon.sprite = LoadHelper.CreateSprite(petData.Icon);
        }

        public void SetSelected(bool isSelected) {
            _selectedBack.enabled = isSelected;
            _defaultBack.enabled = !isSelected;
        }

        private void SelectPet() {
            //SetSelected(true);
            OnSelected.Dispatch(this);
            EventBus<OnCurrentPetChangedInShop>.Raise(new OnCurrentPetChangedInShop(_petData.Asset));
        }
    }
}
