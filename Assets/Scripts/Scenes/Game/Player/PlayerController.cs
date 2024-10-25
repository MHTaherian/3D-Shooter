using System;
using Scenes.Game.Inventory;
using Scenes.Game.UI.InGameUI.InputManager;
using Scenes.Game.Weapons;
using Zenject;
using UnityEngine;

namespace Scenes.Game.Player
{
    public class PlayerController : MonoBehaviour, ITransformGetter
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _animatorTurnSpeed;

        [Space] [Header("WeaponAttachSlots")] [SerializeField]
        private Transform _rifleAttachSlot;
        [SerializeField] private Transform _pistolAttachSlot;
        
        [Space] [Header("Animation")] [SerializeField]
        private Animator _animator;
        [SerializeField] private AnimatorOverrideController _pistolAnimatorOverride;
        [SerializeField] private AnimatorOverrideController _rifleAnimatorOverride;
        
        private Vector2 _moveInputValue;
        private Vector2 _aimInputValue;
        private Camera _mainCamera;
        private float _currentAnimatorTurnSpeed;

        #region AnimatorProperties

        private static readonly int ForwardSpeedAnimationProperty = Animator.StringToHash("forwardSpeed");
        private static readonly int RightSpeedAnimationProperty = Animator.StringToHash("rightSpeed");
        private static readonly int TurnSpeedAnimatorProperty = Animator.StringToHash("turnSpeed");

        #endregion

        private InventoryManager _inventoryManager;
        private Weapon _ownedWeapon;

        public event Action<Vector2> PlayerMoved;

        [Inject]
        private void Init(InventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public void OnMoveInputUpdated(Vector2 inputValue)
        {
            _moveInputValue = inputValue;
        }

        public void OnAimInputUpdated(Vector2 inputValue)
        {
            _aimInputValue = inputValue;
        }

        private void Update()
        {
            var moveDirection = MovePlayerBasedOnMoveStickInputValue();
            RotatePlayerBasedOnAimDirection(moveDirection);

            // moveDirection * transform.forward = |moveDirection| * |transform.forward| * cos(angle between them)
            // which draws a line from tip of moveDirection vector perpendicular to transform.forward
            // and tells us the ratio of moveDirection vector to transform.forward
            var forwardMovementRatio = Vector3.Dot(moveDirection, transform.forward);
            var rightMovementRatio = Vector3.Dot(moveDirection, transform.right);
            _animator.SetFloat(ForwardSpeedAnimationProperty, forwardMovementRatio);
            _animator.SetFloat(RightSpeedAnimationProperty, rightMovementRatio);
        }

        #region Rotation

        private void RotatePlayerBasedOnAimDirection(Vector3 moveDirection)
        {
            var aimDirection = moveDirection;
            if (IsAiming())
            {
                aimDirection = JoystickInputToWorldPosition(_aimInputValue);
            }

            float currentFinalTurnSpeed = 0;
            if (aimDirection.sqrMagnitude > 0)
            {
                var previousRotation = transform.rotation;
                var currentRotation = RotatePlayer(aimDirection);
                var rightAimDirectionRatio = Vector3.Dot(aimDirection, transform.right) > 0 ? 1 : -1;
                var rotationDelta = Quaternion.Angle(previousRotation, currentRotation) * rightAimDirectionRatio;
                currentFinalTurnSpeed = rotationDelta / Time.deltaTime;
            }

            _currentAnimatorTurnSpeed = Mathf.Lerp(_currentAnimatorTurnSpeed, currentFinalTurnSpeed,
                Time.deltaTime * _animatorTurnSpeed);
            _animator.SetFloat(TurnSpeedAnimatorProperty, _currentAnimatorTurnSpeed);
        }

        private Quaternion RotatePlayer(Vector3 aimDirection)
        {
            var turnLerpAlpha = _turnSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimDirection),
                turnLerpAlpha);
            return transform.rotation;
        }

        #endregion

        #region Movement

        private Vector3 MovePlayerBasedOnMoveStickInputValue()
        {
            var moveDirection = JoystickInputToWorldPosition(_moveInputValue);
            _characterController.Move(moveDirection * (Time.deltaTime * _moveSpeed));
            if (IsMoving())
            {
                PlayerMoved?.Invoke(_moveInputValue);
            }

            return moveDirection;
        }

        private bool IsMoving()
        {
            return _moveInputValue.sqrMagnitude > 0;
        }

        #endregion

        private Vector3 JoystickInputToWorldPosition(Vector2 inputValue)
        {
            var rightDirection = _mainCamera.transform.right;
            // Vector3.Cross returns a vector perpendicular to two given vectors
            var upDirection = Vector3.Cross(rightDirection, Vector3.up);
            var moveDirection = rightDirection * inputValue.x + upDirection * inputValue.y;
            return moveDirection;
        }

        public bool IsAiming()
        {
            return _aimInputValue.sqrMagnitude > 0;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        #region Weapon

        public void AddWeaponToOwnedItems(WeaponType weaponType)
        {
            var slotTransform = GetWeaponSlotTransform(weaponType);
            _inventoryManager.AddWeaponToOwnedWeapons(weaponType, slotTransform);
        }

        private Transform GetWeaponSlotTransform(WeaponType weaponType)
        {
            return weaponType switch
            {
                WeaponType.Rifle => _rifleAttachSlot,
                WeaponType.Pistol => _pistolAttachSlot,
                _ => throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null)
            };
        }

        public void SetWeapon(WeaponType weaponType)
        {
            if (_ownedWeapon)
                _ownedWeapon.UnEquip();
            
            _animator.runtimeAnimatorController = GetAnimatorOverrideController(weaponType);
            _ownedWeapon = _inventoryManager.GetWeapon(weaponType);
            _ownedWeapon.Init(gameObject);
        }

        private AnimatorOverrideController GetAnimatorOverrideController(WeaponType weaponType)
        {
            return weaponType switch
            {
                WeaponType.Rifle => _rifleAnimatorOverride,
                WeaponType.Pistol => _pistolAnimatorOverride,
                WeaponType.None => default,
                _ => throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null)
            };
        }

        #endregion
    }
}