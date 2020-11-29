using DG.Tweening;
using Runtime.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Equipment {
    public class Equipment : UiBase {
        private enum EquipmentCategory {
            Inventory,
            Pets,
            Skins
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

        private Camera _camera;

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
            _skins.OnBackClick.AddListener(OnCloseSubMenu);
        }

        private void OnCloseSubMenu() {
            SelectCategory(EquipmentCategory.Inventory);
        }

        private void SelectCategory(EquipmentCategory category) {
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

        private void OnDestroy() {
            _changePetButton.onClick.RemoveListener(OpenPetsScene);
            _changeSkinButton.onClick.RemoveListener(OpenSkinsScene);
            _petsShop.OnBackClick.RemoveListener(OnCloseSubMenu);
            _skins.OnBackClick.RemoveListener(OnCloseSubMenu);
        }
    }
}