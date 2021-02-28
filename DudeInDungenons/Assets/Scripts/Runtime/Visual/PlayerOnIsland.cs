using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;
using Runtime.Static;
using UnityEngine;

namespace Runtime.Visual {
    public class PlayerOnIsland : MonoBehaviour, IEventReceiver<OnCurrentItemChangedInShop> {
        public enum PlayerAnimation {
            Sitting,
            Idle
        }
        
        [SerializeField]
        private PlayerAnimation _startAnimation = PlayerAnimation.Idle;
        
        private Animator _animator;
        private GameController _gameController;
        private GameObject _currentSkin;
        private Item _currentSkinItem;
        private Transform _root;
        private bool _skinIsLoading;

        private void Start() {
            EventBus.Register(this);
            
            _root = transform.Find("Root");
            _gameController = GameObject.FindWithTag(EntityTag.GameController.ToString()).GetComponent<GameController>();

            UpdateSkin(_gameController.ItemReference.GetItem(_gameController.Progress.Player.CurrentSkin));
        }

        public void UpdateSkin(Item item) {
            if (_skinIsLoading) {
                return;
            }

            if (item is null) {
                return;
            }

            if (_currentSkin != null) {
                if (item.Id == _currentSkinItem.Id) {
                    return;
                }

                _currentSkinItem.Asset.ReleaseInstance(_currentSkin);
            }

            _skinIsLoading = true;

            item.Asset.InstantiateAsync(_root).Completed += handle => {
                var go = handle.Result;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                _currentSkin = go;
                _currentSkinItem = item;
                _animator = GetComponentInChildren<Animator>();
                SetAnimation(_startAnimation);
                
                _skinIsLoading = false;
            };
        }

        private void SetAnimation(PlayerAnimation animationToPlay) {
            switch (animationToPlay) {
                case PlayerAnimation.Sitting:
                    _animator.SetBool(animationToPlay.ToString(), true);
                    break;
            }
        }

        public void OnEvent(OnCurrentItemChangedInShop e) {
            if (e.ItemType == ItemsReference.ItemType.PlayerSkins) {
                UpdateSkin(e.Data);
            }
        }

        private void OnDestroy() {
            EventBus.UnRegister(this);
        }
    }
}