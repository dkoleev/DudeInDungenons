using Runtime.UI.Base;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu {
    public class MainMenu : UiBase {
        public enum MenuCategory {
            World,
            Equipment,
            Shop
        }
        
        [Title("Cameras")]
        [SerializeField, Required]
        private Camera _worldCamera;
        [Title("Panels")]
        [SerializeField, Required]
        private ResourcesPanel _resourcesPanel;
        [SerializeField, Required]
        private HomeButtonsMenu _menuButtons;
        [SerializeField, Required]
        private Shop _shop;
        [SerializeField, Required]
        private GameObject _worldCategory;
        [SerializeField, Required]
        private GameObject _equipmentCategory;
        [SerializeField, Required]
        private GameObject _shopCategory;
        [SerializeField, Required]
        private Button _worldButton;
        [SerializeField, Required]
        private Button _equipementButton;
        [SerializeField, Required]
        private Button _shopButton;
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
            _shop.Initialize(GameController, ItemsReference);
            _menuButtons.Initialize(this);
            
            SelectCategory(MenuCategory.World);
            InitializeMainButtons();
            
            _inventory.Initialize(GameController, ItemsReference);
            _playButton.onClick.AddListener(() => {
                OnPlayClick.Dispatch();
            });
            
            _testPurchaseButton.onClick.AddListener(() => {
                GameController.Billing.BuyConsumable(BillingManager.PurchaseProducts.gem_0.ToString());
            });

            Initialized = true;
        }

        private void InitializeMainButtons() {
            _worldButton.onClick.AddListener(() => {
                SelectCategory(MenuCategory.World);
            });
            
            _equipementButton.onClick.AddListener(() => {
                SelectCategory(MenuCategory.Equipment);
            });
            
            _shopButton.onClick.AddListener(() => {
                SelectCategory(MenuCategory.Shop);
            });
        }

        public void SelectCategory(MenuCategory category) {
            if (_currentCategory == category) {
                return;
            }

            _currentCategory = category;
            
            _worldCategory.SetActive(_currentCategory == MenuCategory.World);
            _equipmentCategory.SetActive(_currentCategory == MenuCategory.Equipment);
            _shopCategory.SetActive(_currentCategory == MenuCategory.Shop);

            _worldCamera.enabled = _currentCategory != MenuCategory.Shop;
        }
    }
}