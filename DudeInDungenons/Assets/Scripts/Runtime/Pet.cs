using System.Collections;
using Runtime.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime {
    public class Pet : Entity {
        [SerializeField, Required]
        private PetData _data;

        public AssetReference Asset => _data.Asset;
        
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
                var wait = _isSitting ? Random.Range(5, 7) : Random.Range(10, 20);
                yield return new WaitForSeconds(wait);

                _isSitting = !_isSitting;
                _animator.SetBool(Sitting, _isSitting);
            }
        }
    }
}