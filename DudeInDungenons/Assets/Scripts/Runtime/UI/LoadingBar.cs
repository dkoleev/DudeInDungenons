using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui {
    public class LoadingBar : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI _textDownload;
        [SerializeField]
        private Slider _sliderDownload;
        
        private CanvasGroup _canvasGroup;
        private int maxSize;

        public void InIt(int maxSize) {
            this.maxSize = maxSize;
            _sliderDownload.maxValue = maxSize;

            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.DOFade(0f, 0f);
        }

        public void Show() {
            gameObject.SetActive(true);
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutCubic);
        }

        public void Hide() {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).OnComplete(() => {
                gameObject.SetActive(false);
            });
        }

        public void SetBar(int value) {
            /*_textDownload.text = string.Format("<color=#00FFFF>Downloading...</color>{0}MB/{1}MB",
                Utility.ChangeMoneyString(value), Utility.ChangeMoneyString(maxSize));*/
            _sliderDownload.value = value;
        }
    }
}