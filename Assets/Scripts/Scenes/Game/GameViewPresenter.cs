using Scenes.Game.GameCamera;
using Scenes.Game.Player;
using Scenes.Game.UI.InGameUI;
using UnityEngine;

namespace Scenes.Game
{
    public class GameViewPresenter
    {
        private readonly PlayerController _playerController;
        private readonly CameraController _cameraController;

        public GameViewPresenter(PlayerController playerController, CameraController cameraController,
            InGameUi inGameUi)
        {
            _playerController = playerController;
            _cameraController = cameraController;
            cameraController.SetTransformToFollow(playerController);
            playerController.PlayerMoved += OnPlayerMoved;
            inGameUi.MoveStickValueUpdated += playerController.OnMoveInputUpdated;
            inGameUi.AimStickValueUpdated += playerController.OnAimInputUpdated;
        }

        private void OnPlayerMoved(Vector2 moveInput)
        {
            if (_playerController.IsAiming()) return;
            _cameraController.RotateFrameRateIndependently(moveInput.x);
        }
    }
}