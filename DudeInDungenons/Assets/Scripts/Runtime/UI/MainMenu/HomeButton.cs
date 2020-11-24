using DG.Tweening;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu {

    public class HomeButton : MonoBehaviour {
        [SerializeField]
        private MainMenu.MenuCategory _category;
        [SerializeField, Required]
        private Image _icon;
        [SerializeField, Required]
        private TextMeshProUGUI _description;
        [SerializeField, Required]
        private Image _background;

        public Relay<MainMenu.MenuCategory> OnClick = new Relay<MainMenu.MenuCategory>();
        
        public MainMenu.MenuCategory Category => _category;
        
        private Button _button;
        private bool _isActive;
        private Sequence _sequence;
        private bool _initialized;
        private RectTransform _rect;
        private Vector2 _rectStartSize;

        public void Initialize() {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClickButton);
            _rect = GetComponent<RectTransform>();
            _rectStartSize = _rect.sizeDelta;
        }

        private void OnClickButton() {
            OnClick.Dispatch(_category);
        }

        public void SetActive(bool isActive, bool withAnimation = true) {
            if (_isActive == isActive && _initialized) {
                return;
            }

            _isActive = isActive;
            _initialized = true;

            if (_sequence != null && _sequence.IsPlaying()) {
                _sequence.Complete(true);
            }

            var delay = 0.5f;

            _sequence = DOTween.Sequence();
            if (_isActive) {
                _sequence.Insert(0,_description.DOFade(1, withAnimation ? delay : 0)).SetEase(Ease.OutCubic);
                _sequence.Insert(0,_icon.transform.DOLocalMoveY(50, withAnimation ? delay : 0)).SetEase(Ease.OutCubic);
                _sequence.Insert(0,_icon.transform.DOScale(1.2f, withAnimation ? delay : 0)).SetEase(Ease.OutElastic);
                _sequence.Insert(0,_rect.DOSizeDelta(new Vector2(300, _rectStartSize.y), withAnimation ? delay : 0)).SetEase(Ease.OutCubic);
            } else {
                _sequence.Insert(0,_description.DOFade(0, withAnimation ? delay : 0)).SetEase(Ease.OutCubic);
                _sequence.Insert(0,_icon.transform.DOLocalMoveY(10f, withAnimation ? delay : 0)).SetEase(Ease.OutCubic);
                _sequence.Insert(0,_icon.transform.DOScale(1.0f, withAnimation ? delay : 0)).SetEase(Ease.OutCubic);
                _sequence.Insert(0,_rect.DOSizeDelta(new Vector2(_rectStartSize.x, _rectStartSize.y), withAnimation ? delay : 0)).SetEase(Ease.OutCubic);
            }
        }

        private void OnDestroy() {
            _button.onClick.RemoveListener(OnClickButton);
        }
    }
}