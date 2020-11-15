using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World {
    public class WorldBar : MonoBehaviour, IDisposable {
        [SerializeField] private Image _progress;
        [SerializeField] private Image _bufferProgress;
        [SerializeField] private TextMeshPro _amount;
        [SerializeField] private bool _showAmount;

        private float _currentValue;
        private float _maxValue;

        public void Initialize(float currentValue, float maxValue) {
            _maxValue = maxValue;
            SetProgress(currentValue, 0.0f);
            _amount.enabled = _showAmount;
        }

        public void SetProgress(float value, float animationSpeed = 1.0f) {
            _currentValue = value;
            _amount.text = _currentValue.ToString();
            var value01 = (float) value / _maxValue;
            _progress.fillAmount = Mathf.Clamp01(value01);
            _bufferProgress.DOFillAmount(value01, animationSpeed);
        }

        public void Dispose() {
            DOTween.Kill(_bufferProgress);
            gameObject.SetActive(false);
        }
    }
}