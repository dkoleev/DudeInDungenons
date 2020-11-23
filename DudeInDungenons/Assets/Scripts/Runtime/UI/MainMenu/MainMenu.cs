using Runtime.Logic.GameProgress;
using Runtime.UI.Base;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu {
    public class MainMenu : UiBase {
        private enum MenuCategory {
            World,
            Equipment
        }

        [SerializeField, Required]
        private ResourcesPanel _resourcesPanel;
        [SerializeField, Required]
        private GameObject _worldCategory;
        [SerializeField, Required]
        private GameObject _equipmentCategory;
        [SerializeField, Required]
        private Button _worldButton;
        [SerializeField, Required]
        private Button _equipementButton;
        [SerializeField, Required]
        private Inventory _inventory;
        [SerializeField, Required]
        private Button _playButton;

        [SerializeField, Required]
        private Button _testPurchaseButton;
        
        
        public Relay OnPlayClick = new Relay();

        private MenuCategory _currentCategory = MenuCategory.Equipment;

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);
            
            _resourcesPanel.Initialize(GameController, ItemsReference);
            
            SelectCategory(MenuCategory.World);
            InitializeMainButtons();
            
            _inventory.Initialize(GameController, ItemsReference);
            _playButton.onClick.AddListener(() => {
                OnPlayClick.Dispatch();
            });
            
            _testPurchaseButton.onClick.AddListener(() => {
                GameController.Billing.BuyConsumable(BillingManager._product_gem_0);
            });
        }

        private void InitializeMainButtons() {
            _worldButton.onClick.AddListener(() => {
                SelectCategory(MenuCategory.World);
            });
            
            _equipementButton.onClick.AddListener(() => {
                SelectCategory(MenuCategory.Equipment);
            });
        }

        private void SelectCategory(MenuCategory category) {
            if (_currentCategory == category) {
                return;
            }

            _currentCategory = category;
            
            _worldCategory.SetActive(_currentCategory == MenuCategory.World);
            _equipmentCategory.SetActive(_currentCategory == MenuCategory.Equipment);
        }
    }
}