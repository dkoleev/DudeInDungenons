using DG.Tweening;
using Runtime.Logic;
using Runtime.UI.Base;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Equipment {
    public class Equipment : UiBase {
        private enum EquipmentCategory {
            Inventory,
            Pets,
            Skins,
            None
        }
        
        [SerializeField, Required]
        private Inventory _inventory;
        [SerializeField, Required]
        private PetsShop _petsShop;
        [SerializeField, Required]
        private Skins _skins;
        [SerializeField, Required]
        private Button _changePetButton;
        [SerializeField, Required]
        private Button _changeSkinButton;

        [Title("Camera Settings")]
        [SerializeField]
        private Ease _moveEase;
        [SerializeField]
        private float _moveDuration = 1.0f;
        
        public Relay<string> OnNeedResources = new Relay<string>();

        private Camera _camera;
        private EquipmentCategory _currentCategory;

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            _camera = GameObject.FindWithTag("Camera_Hero_Menu").GetComponent<Camera>();
            
            _inventory.Initialize(GameController, ItemsReference);
            _petsShop.Initialize(GameController, ItemsReference);
            _skins.Initialize(GameController, ItemsReference);
            
            SelectCategory(EquipmentCategory.Inventory);
            
            _changePetButton.onClick.AddListener(OpenPetsScene);
            _changeSkinButton.onClick.AddListener(OpenSkinsScene);
            _petsShop.OnBackClick.AddListener(OnCloseSubMenu);
            _petsShop.OnNeedResources.AddListener(NoResources);
            _skins.OnBackClick.AddListener(OnCloseSubMenu);
        }

        private void NoResources(string resourceId) {
            OnNeedResources.Dispatch(resourceId);
        }

        private void OnCloseSubMenu() {
            SelectCategory(EquipmentCategory.Inventory);
        }

        private void SelectCategory(EquipmentCategory category) {
            if (category != EquipmentCategory.None) {
                _currentCategory = category;
            }
            
            _inventory.SetActive(category == EquipmentCategory.Inventory);
            _petsShop.SetActive(category == EquipmentCategory.Pets);
            _skins.SetActive(category == EquipmentCategory.Skins);

            MoveCamera(category);
        }

        private void MoveCamera(EquipmentCategory category) {
            var moveTarget = 0f;
            switch (category) {
                case EquipmentCategory.Skins:
                    moveTarget = -12;
                    break;
                case EquipmentCategory.Inventory:
                    moveTarget = 0;
                    break;
                case EquipmentCategory.Pets:
                    moveTarget = 12;
                    break;
            }

            _camera.transform.DOLocalMoveX(moveTarget, _moveDuration).SetEase(_moveEase);
        }

        private void OpenPetsScene() {
            SelectCategory(EquipmentCategory.Pets);
        }
        
        private void OpenSkinsScene() {
            SelectCategory(EquipmentCategory.Skins);
        }

        private void OpenShop() {
            SelectCategory(EquipmentCategory.None);
        }

        private void OnDestroy() {
            _changePetButton.onClick.RemoveListener(OpenPetsScene);
            _changeSkinButton.onClick.RemoveListener(OpenSkinsScene);
            _petsShop.OnBackClick.RemoveListener(OnCloseSubMenu);
            _petsShop.OnNeedResources.RemoveListener(NoResources);
            _skins.OnBackClick.RemoveListener(OnCloseSubMenu);
        }
    }
}