using System;
using Scenes.Game.UI.InGameUI.InputManager;
using Zenject;
using UnityEngine;

namespace Scenes.Game.Player
{
    public class PlayerController : MonoBehaviour, ITransformGetter
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;
        private Vector2 _moveInputValue;
        private Vector2 _aimInputValue;
        private Camera _mainCamera;

        public event Action<Vector2> PlayerMoved;

        // [Inject]
        // private void Init()
        // {
        // }

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
        }

        #region Rotation
        
        private void RotatePlayerBasedOnAimDirection(Vector3 moveDirection)
        {
            var aimDirection = moveDirection;
            if (IsAiming())
            {
                aimDirection = JoystickInputToWorldPosition(_aimInputValue);
            }

            if (aimDirection.sqrMagnitude > 0)
            {
                RotatePlayer(aimDirection);
            }
        }
        
        private void RotatePlayer(Vector3 moveDirection)
        {
            var turnLerpAlpha = _turnSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection),
                turnLerpAlpha);
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
    }
}