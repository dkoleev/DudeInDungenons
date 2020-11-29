using System.Collections.Generic;
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
        private Image _currentFocus;
        [SerializeField, Required]
        private List<Image> _defaultBack;
        [SerializeField, Required]
        private List<Image> _selectedBack;

        public bool IsSelected { get; private set; }
        public PetData Data => _petData;

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

        public void SetCurrent(string currentPetId) {
            _currentFocus.enabled = _petData.Asset.AssetGUID == currentPetId;
        }

        public void SetSelected(bool isSelected) {
            IsSelected = isSelected;
            
            _selectedBack.ForEach(image => image.enabled = IsSelected);
            _defaultBack.ForEach(image => image.enabled = !IsSelected);
        }
        
        private void SelectPet() {
            //SetSelected(true);
            OnSelected.Dispatch(this);
            EventBus<OnCurrentPetChangedInShop>.Raise(new OnCurrentPetChangedInShop(_petData.Asset));
        }
    }
}
