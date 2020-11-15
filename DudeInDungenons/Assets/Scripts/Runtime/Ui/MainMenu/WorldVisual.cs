using DG.Tweening;
using Runtime.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Ui.MainMenu {
    public class WorldVisual : MonoBehaviour {
        [SerializeField,Required]
        private WorldData _data;
        [SerializeField]
        private Transform _floatAnimationTarget;

        [SerializeField]
        private float _animationSpeed = 1.0f;
        [SerializeField]
        private float _floatingOffset = 10.0f;

        public WorldData Data => _data;
        
        void Start() {
            AnimateFloating();
        }

        private void AnimateFloating() {
            var currentPosition = transform.localPosition;
            if (_floatAnimationTarget == null) {
                _floatAnimationTarget = transform;
            }
            
            _floatAnimationTarget.DOMoveY(currentPosition.y + _floatingOffset, _animationSpeed).
                SetLoops(-1, LoopType.Yoyo).
                SetEase(Ease.InOutQuad);
        }
    }
}