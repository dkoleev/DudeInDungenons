using DG.Tweening;
using Runtime.Logic;
using UnityEngine;

namespace Runtime.Game.Entities.World.Traps {
    public class BuzzSawTrap : Entity {
        [SerializeField]
        private Saw _saw;
        [SerializeField]
        private float _sawMoveSpeed;
        [SerializeField]
        private float _sawRotateSpeed;
        [SerializeField]
        private int _damage;

        protected override void Awake() {
            base.Awake();

            _saw.OnTriggerEnterAction += OnTriggerEnter;

            _saw.transform.localPosition = new Vector3(-2.7f, 0);
            _saw.transform.DOLocalMoveX(2.7f, _sawMoveSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            _saw.transform.DOLocalRotate(new Vector3(0, 0, 180), _sawRotateSpeed).SetLoops(-1).SetEase(Ease.Linear);
        }

        private void OnTriggerEnter(Collider other) {
            var damagable = other.GetComponent<IDamagable>();
            if (damagable is null) {
                return;
            }
            
            damagable.TakeDamage(_damage);
            Debug.LogError("trigger enter");
        }
    }
}