using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World {
    public class WorldBar : MonoBehaviour {
        [SerializeField] private Image _progress;
        [SerializeField] private Image _bufferProgress;

        private void Initialize() {
            /*var value = (float) _health.Model.CurrentHealth / _health.Model.MaxHealth;
            SetProgress(value, value);
            _health.Model.OnHealthChange.AddListener((prev, current, max) => {
                var prevValue = (float)prev / max;
                var newValue = (float)current / max;
                SetProgress(newValue, prevValue);
            });
            _health.Model.OnDead.AddListener(component => {
                gameObject.SetActive(false);
            });*/
        }

        private void SetProgress(float value, float prevValue) {
            /*_progress.fillAmount = Mathf.Clamp01(value);
            _bufferProgress.DOFillAmount(value, 1.0f);*/
        }

        private void Update() {
            /*base.Update();
            transform.eulerAngles = Vector3.zero;*/
        }
    }
}