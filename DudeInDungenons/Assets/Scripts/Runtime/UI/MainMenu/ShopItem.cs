using System.Linq;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.UI.Base;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu {
    public class ShopItem : UiBase, IEventReceiver<OnBillingInitialized> {
        [SerializeField]
        private bool _storeProduct = true;
        [SerializeField]
        [HideIf("_storeProduct")]
        private string _id;
        [SerializeField]
        [ShowIf("_storeProduct")]
        private BillingManager.PurchaseProducts _productId;
        [SerializeField, Required]
        private TextMeshProUGUI _title;
        [SerializeField, Required]
        private Text _price;
        [SerializeField]
        private TextMeshProUGUI _priceResource;
        [SerializeField, Required]
        private TextMeshProUGUI _reward;
        [SerializeField, Required]
        private Button _buyButton;
        
        void Start() {
            EventBus.Register(this);
            
            _buyButton.onClick.AddListener(ByProduct);
        }

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            if (GameController.Billing.IsInitialized()) {
                SetPrice();
            }

            if (_storeProduct) {
                var itemData = GameController.Billing.Data.StoreItems.First(store => store.Price == _productId);
                _reward.text = itemData.Reward.Amount.ToString();
                _title.text = itemData.Title;
            } else {
                var itemData = GameController.Billing.Data.ResourceItems.First(store => store.Id == _id);
                _reward.text = itemData.Reward.Amount.ToString();
                _title.text = itemData.Title;
            }
        }

        private void SetPrice() {
            if (_storeProduct) {
                _price.text = GameController.Billing.GetPriceString(_productId.ToString());
            } else {
                var itemData = GameController.Billing.Data.ResourceItems.First(store => store.Id == _id);
                _priceResource.text = itemData.Price.Amount.ToString();
            }
        }

        private void ByProduct() {
            if (_storeProduct) {
                GameController.Billing.BuyConsumable(_productId.ToString());
            } else {
                var itemData = GameController.Billing.Data.ResourceItems.First(store => store.Id == _id);

                if (GameController.Inventory.SpendResource(itemData.Price) ==
                    Logic.Inventory.Inventory.InventoryOperationResult.Success) {
                    GameController.Inventory.Add(itemData.Reward.Item.Id, itemData.Reward.Amount);
                }
            }
        }

        public void OnEvent(OnBillingInitialized e) {
            SetPrice();
        }
        
        private void OnDestroy() {
            _buyButton.onClick.RemoveListener(ByProduct);
            EventBus.UnRegister(this);
        }
    }
}