using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World {
    public class WorldBar : MonoBehaviour {
        [SerializeField] private Image _progress;
        [SerializeField] private Image _bufferProgress;

        private float _currentValue;
        private float _maxValue;

        public void Initialize(float currentValue, float maxValue) {
            _currentValue = currentValue;
            _maxValue = maxValue;
            SetProgress(_currentValue);
        }

        public void SetProgress(float value) {
            var value01 = (float) value / _maxValue;
            
            _progress.fillAmount = Mathf.Clamp01(value01);
            _bufferProgress.DOFillAmount(value01, 1.0f);
        }
    }
}