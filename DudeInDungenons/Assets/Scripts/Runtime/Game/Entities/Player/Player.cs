using System;
using System.Collections.Generic;
using Avocado.Framework.Patterns.StateMachine;
using Runtime.Data;
using Runtime.Data.Items;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.GameProgress.Progress;
using Runtime.Logic.States.Player;
using Runtime.Static;
using Runtime.Ui.World;
using Runtime.Utilities;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Game.Entities.Player {
    public class Player : Entity, ILocalPositionAdapter, IWeaponOwner, IDamagable,
        IEventReceiver<OnEnemyDead>,
        IEventReceiver<OnLevelCompleted> {

        public enum PlayerState {
            Idle,
            Run,
            Attack
        }

        [SerializeField, Required, AssetsOnly, InlineEditor]
        private PlayerData _data;

        [SerializeField]
        public EntityTag _attackTarget;

        [SerializeField, Required]
        private Transform _shootRaycastStartPoint;

        public Relay<IState, IState> OnStateChanged = new Relay<IState, IState>();

        public Vector3 LocalPosition {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Transform RaycastStartPoint => _shootRaycastStartPoint;
        public Transform RotateTransform => _rotateTransform;
        public Transform Root => _rotateTransform;
        public Transform MainTransform => _mainTransform;
        public bool IsMoving => _mover.IsMoving;
        public Dictionary<string, int> Drop => _drop;


        private WorldBar _healthBar;
        private MoveByController _mover;
        public AttackComponent AttackComponent { get; private set; }
        private RotateByAxis _rotator;
        private FindTargetByDistance _findTargetByDistance;
        private PlayerVisual _visual;

        private Transform _rotateTransform;
        private Transform _root;
        private Transform _mainTransform;
        private PlayerProgress PlayerProgress => Progress.Player;
        private int _health;

        private StateMachine _stateMachine;
        private IState _attackState;
        private Dictionary<string, int> _drop = new Dictionary<string, int>();

        private bool _initialized;
        private bool _isStopping;

        protected override void Awake() {
            base.Awake();
            EventBus.Register(this);

            _health = _data.MaxHealth;
            _mainTransform = transform;
            _root = _mainTransform.Find("Root");
            _rotateTransform = _root;

            _visual = new PlayerVisual(this);

            var currentWeapon = GetEquippedWeapon();
            if (!string.IsNullOrEmpty(currentWeapon)) {
                AttackComponent = new AttackComponent(currentWeapon, this, _data.SpeedRotateNoMove);
                AddComponent(AttackComponent);
            }

            _mover = new MoveByController(this, _data.SpeedMove);
            AddComponent(_mover);
            _rotator = new RotateByAxis(_rotateTransform, _data.SpeedRotate);
            AddComponent(_rotator);
            _findTargetByDistance = new FindTargetByDistance(_mainTransform, _attackTarget);
            AddComponent(_findTargetByDistance);

            _healthBar = GetComponentInChildren<WorldBar>();
            _healthBar.Initialize(_health, _data.MaxHealth);

            AttackComponent?.OnShoot.AddListener(OnShoot);
        }
        
        protected override void Start() {
        }

        public override void Initialize(GameController gameController) {
            base.Initialize(gameController);

            var currentSkinData = GetSkin(GameController);
            currentSkinData.Asset.InstantiateAsync(_root).Completed += handle => {
                InitializeComponents();

                var go = handle.Result;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;

                _visual.Initialize();

                InitializeFsm();
                InitializePet();

                _initialized = true;
            };
        }
        
        private void InitializePet() {
            if (string.IsNullOrEmpty(PlayerProgress.CurrentPet)) {
                return;
            }

            LoadHelper.InstantiateAsset<Pet.Pet>(PlayerProgress.CurrentPet, GameController.ItemReference, pet => {
                pet.transform.position = transform.position + new Vector3(2, 0, 0);
            });
        }
        
        public void Resurrect() {
            AddHealth(_data.MaxHealth);
        }

        private string GetEquippedWeapon() {
            foreach (var itemStack in _data.StartInventory) {
                if (itemStack.Equipped && itemStack.Item is WeaponData) {
                    return itemStack.Item.Id;
                }
            }

            return String.Empty;
        }

        public static Item GetSkin(GameController gameController, string skinId = "") {
            var playerData = gameController.PlayerData;

            var skin = skinId;
            if (string.IsNullOrEmpty(skin)) {
                skin = string.IsNullOrEmpty(gameController.Progress.Player.CurrentSkin)
                    ? playerData.StartSkin.Id
                    : gameController.Progress.Player.CurrentSkin;
            }
           
            var data = gameController.ItemReference.GetItem(ItemsReference.ItemType.PlayerSkins, skin);

            return data;
        }

        private void InitializeFsm() {
            _stateMachine = new StateMachine();
            var idleState = new PlayerIdleState();
            var moveState = new PlayerMoveState(_rotator, _mover, _findTargetByDistance);
            _attackState = new PlayerAttackState(AttackComponent, _findTargetByDistance);

            _stateMachine.SetState(idleState);

            To(_attackState, () => !_mover.IsMoving && _findTargetByDistance.CurrentTargetIsAvailable());
            To(moveState, () => _mover.IsMoving);
            To(idleState, () => !_mover.IsMoving && !_findTargetByDistance.CurrentTargetIsAvailable());

            void To(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

            _stateMachine.OnStateChanged += (prevState, newState) => {
                OnStateChanged.Dispatch(prevState, newState);
            };
        }

        protected override void Update() {
            base.Update();

            if (!_initialized || _isStopping) {
                return;
            }

            _mover.Update();
            _findTargetByDistance.Update();
            _stateMachine.Tick();
            _visual.Update();

            _mainTransform.localRotation = Quaternion.Euler(new Vector3(0, _mainTransform.localRotation.y, 0));
        }

        private void OnShoot() {
            _stateMachine.SetState(_attackState);
            _visual.UpdateVisualByState(null, _attackState);
        }

        public void TakeDamage(int damage) {
            _health -= damage;
            if (_health <= 0) {
                _health = 0;
                Dead();
            }

            _healthBar.SetProgress(_health);
        }

        public void AddHealth(int amount) {
            _health += amount;
            if (_health > _data.MaxHealth) {
                _health = _data.MaxHealth;
            }

            _healthBar.SetProgress(_health);
        }

        private void AddDrop(List<ItemStack> drop) {
            foreach (var itemStack in drop) {
                var id = itemStack.Item.Id;
                if (_drop.ContainsKey(id)) {
                    _drop[id] += itemStack.Amount;
                } else {
                    _drop.Add(id, itemStack.Amount);
                }
            }
        }

        private void Dead() {
            EventBus<OnPlayerDead>.Raise(new OnPlayerDead());
        }

        public void OnEvent(OnEnemyDead e) {
            AddDrop(e.Enemy.Data.Drop);
        }

        public void OnEvent(OnLevelCompleted e) {
            _isStopping = true;
        }

        private void OnDestroy() {
            AttackComponent.OnShoot.RemoveListener(OnShoot);
        }
    }
}