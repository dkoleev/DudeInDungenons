using System.Collections;
using UnityEngine;

namespace Runtime {
    public class Pet : Entity {
        private Animator _animator;
        private static readonly int Sitting = Animator.StringToHash("Sitting");

        private bool _isSitting;

        protected override void Awake() {
            base.Awake();
            
            _animator = GetComponentInChildren<Animator>();

            _isSitting = false;
            StartCoroutine(ChangeState());
        }

        protected override void Start() {
            base.Start();
        }

        private IEnumerator ChangeState() {
            while (true) {
                var wait = _isSitting ? Random.Range(3, 5) : Random.Range(10, 20);
                yield return new WaitForSeconds(wait);

                _isSitting = !_isSitting;
                _animator.SetBool(Sitting, _isSitting);
            }
        }
    }
}