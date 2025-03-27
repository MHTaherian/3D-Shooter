using System;
using Scenes.Game.Framework;
using Scenes.Game.Framework.Creature;
using Scenes.Game.Framework.Creature.Containers;
using Scenes.Game.Framework.Creature.Enemy.Chomper;
using Scenes.Game.Framework.Creature.Managers;
using Scenes.Game.Framework.Creature.Player;
using Scenes.Game.GameCamera;
using Scenes.Game.Inventory;
using Scenes.Game.UI.InGameUI;
using Scenes.Game.Weapons;
using UnityEngine;
using Zenject;

namespace Scenes.Game
{
    public class GameViewPresenter
    {
        private PlayerPresenter _playerPresenter;
        private readonly CreaturesManager _creaturesManager;
        private CameraController _cameraController;
        private GameView _gameView;
        
        public GameViewPresenter(CreaturesManager creaturesManager, CameraController cameraController,
            InGameUi inGameUi, GameView gameView)
        {
            _creaturesManager = creaturesManager;
            _cameraController = cameraController;
            _gameView = gameView;
            _playerPresenter = _creaturesManager.CreateCreature<PlayerPresenter>(CreatureType.Human);
            _playerPresenter.CreateCreatureComponent(gameView.GetPlayerSpawnPosition(), gameView.transform);
            _cameraController.SetTransformToFollow(_playerPresenter.GetPlayerTransformGetter());
            _playerPresenter.AddWeaponToOwnedItems(WeaponType.Rifle);
            _playerPresenter.AddWeaponToOwnedItems(WeaponType.Pistol);
            _playerPresenter.SetWeapon(WeaponType.Pistol);
            _playerPresenter.PlayerMoved += OnPlayerMoved;
            _playerPresenter.AttackedTarget += PlayerPresenterOnAttackedTarget;
            inGameUi.MoveStickValueUpdated += _playerPresenter.OnMoveInputUpdated;
            inGameUi.AimStickValueUpdated += _playerPresenter.OnAimInputUpdated;
            inGameUi.AimStickTapped += _playerPresenter.OnAimStickTapped;

            var chomperPresenter = _creaturesManager.CreateCreature<ChomperPresenter>(CreatureType.Chomper);
            chomperPresenter.CreateCreatureComponent(gameView.GetChomperSpawnPosition(), gameView.transform);
        }

        private void OnPlayerMoved(Vector2 moveInput)
        {
            if (_playerPresenter.IsAiming()) return;
            _cameraController.RotateFrameRateIndependently(moveInput.x);
        }

        private void PlayerPresenterOnAttackedTarget(GameObject target, float damagePerBullet)
        {
            Debug.Log($"Attacked {target}");
            if (target.TryGetComponent(out CreatureComponent creatureComponent))
            {
                creatureComponent.OnGotAttacked(damagePerBullet);
            }
        }
    }
}