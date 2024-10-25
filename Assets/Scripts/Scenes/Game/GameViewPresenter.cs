using Scenes.Game.GameCamera;
using Scenes.Game.Inventory;
using Scenes.Game.Player;
using Scenes.Game.UI.InGameUI;
using Scenes.Game.Weapons;
using UnityEngine;
using Zenject;

namespace Scenes.Game
{
    public class GameViewPresenter
    {
        private PlayerController _playerController;
        private CameraController _cameraController;

        [Inject]
        public void Init(PlayerController playerController, CameraController cameraController,
            InGameUi inGameUi)
        {
            _playerController = playerController;
            _cameraController = cameraController;
            cameraController.SetTransformToFollow(playerController);
            playerController.AddWeaponToOwnedItems(WeaponType.Rifle);
            playerController.AddWeaponToOwnedItems(WeaponType.Pistol);
            playerController.SetWeapon(WeaponType.Pistol);
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