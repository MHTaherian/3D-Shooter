using System;
using Scenes.Game.UI.InGameUI.InputManager;
using UnityEngine;

namespace Scenes.Game.UI.InGameUI
{
    public class InGameUi : MonoBehaviour
    {
        [SerializeField] private Joystick _moveJoyStick;
        [SerializeField] private Joystick _aimJoyStick;

        public event Action<Vector2> MoveStickValueUpdated;
        public event Action<Vector2> AimStickValueUpdated;
        public event Action AimStickTapped;
        private void Awake()
        {
            _moveJoyStick.OnInputValueUpdated += OnMoveJoystickValueUpdated;
            _aimJoyStick.OnInputValueUpdated += OnAimJoystickValueUpdated;
            _aimJoyStick.OnInputTapped += OnAimJoyStickInputTapped;
        }

        private void OnMoveJoystickValueUpdated(Vector2 inputValue)
        {
            MoveStickValueUpdated?.Invoke(inputValue);
        }

        private void OnAimJoystickValueUpdated(Vector2 inputValue)
        {
            AimStickValueUpdated?.Invoke(inputValue);
        }

        private void OnAimJoyStickInputTapped()
        {
            AimStickTapped?.Invoke();
        }

        private void OnDestroy()
        {
            _moveJoyStick.OnInputValueUpdated -= OnMoveJoystickValueUpdated;
            _aimJoyStick.OnInputValueUpdated -= OnAimJoystickValueUpdated;
            _aimJoyStick.OnInputTapped -= OnAimJoyStickInputTapped;
        }
    }
}