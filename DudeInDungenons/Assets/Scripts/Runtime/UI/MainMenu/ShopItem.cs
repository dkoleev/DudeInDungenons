using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu {
    public class ShopItem : UiBase, IEventReceiver<OnBillingInitialized>, IPointerDownHandler, IPointerUpHandler {
        [SerializeField]
        private BillingManager.PurchaseProducts _productId;
        [SerializeField, Required]
        private Text _price;
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
        }

        private void SetPrice() {
            _price.text = GameController.Billing.GetPriceString(_productId.ToString());
        }

        private void ByProduct() {
           GameController.Billing.BuyConsumable(_productId.ToString());
        }

        public void OnEvent(OnBillingInitialized e) {
            SetPrice();
        }
        
        private void OnDestroy() {
            _buyButton.onClick.RemoveListener(ByProduct);
            EventBus.UnRegister(this);
        }

        public void OnPointerDown(PointerEventData eventData) {
            
        }

        public void OnPointerUp(PointerEventData eventData) {
            ByProduct();
        }
    }
}