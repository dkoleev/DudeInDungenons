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
        private Camera _mainCamera;
        [SerializeField, Required]
        private Camera _worldCamera;
        [SerializeField, Required]
        private Camera _heroCamera;
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
        private Equipment.Equipment _equipment;
        [SerializeField, Required]
        private Button _worldButton;
        [SerializeField, Required]
        private Button _equipementButton;
        [SerializeField, Required]
        private Button _shopButton;
        [SerializeField, Required]
        private Button _playButton;
        [SerializeField, Required]
        private Button _testPurchaseButton;
        
        
        public Relay OnPlayClick = new Relay();

        private MenuCategory _currentCategory = MenuCategory.Equipment;

        private int _worldCameraCullingMask;

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            _worldCameraCullingMask = _worldCamera.cullingMask;
            
            _resourcesPanel.Initialize(GameController, ItemsReference);
            _menuButtons.Initialize(this);
            
            _shop.Initialize(GameController, ItemsReference);
            _equipment.Initialize(GameController, ItemsReference);
            
            SelectCategory(MenuCategory.World);
            InitializeMainButtons();
            
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
            _equipment.SetActive(_currentCategory == MenuCategory.Equipment);
            _shop.SetActive(_currentCategory == MenuCategory.Shop);

            _worldCamera.cullingMask = _currentCategory == MenuCategory.World ? _worldCameraCullingMask : 0;
            _heroCamera.enabled = _currentCategory == MenuCategory.Equipment;
            
            _mainCamera.clearFlags = _currentCategory != MenuCategory.Shop
                ? CameraClearFlags.Skybox
                : CameraClearFlags.Nothing;
        }
    }
}