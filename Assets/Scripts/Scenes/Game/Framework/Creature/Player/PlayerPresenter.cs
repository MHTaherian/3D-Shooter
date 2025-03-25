using System;
using System.Linq;
using Scenes.Game.Framework.Creature.Containers;
using Scenes.Game.Inventory;
using Scenes.Game.Weapons;
using UnityEngine;

namespace Scenes.Game.Framework.Creature.Player
{
    public class PlayerPresenter : Creature
    {
        private readonly PlayerComponent.Factory _factory;
        private readonly InventoryManager _inventoryManager;
        private PlayerComponent _playerComponent;
        private Weapon _currentWeapon;

        public event Action<Vector2> PlayerMoved;

        public event Action<GameObject, float> AttackedTarget;

        public PlayerPresenter(PlayerComponent.Factory factory, InventoryManager inventoryManager,
            CreatureHealthManager healthManager) : base(healthManager, CreatureType.Human)
        {
            _factory = factory;
            _inventoryManager = inventoryManager;
        }

        protected override CreatureComponent CreateComponent(Vector3 position, Transform parent)
        {
            _playerComponent = _factory.Create(position, parent);
            _playerComponent.SwitchedToNextWeapon += PlayerComponentOnSwitchedToNextWeapon;
            _playerComponent.TriedToGetAttackTarget += PlayerComponentOnTriedToGetAttackTarget;
            _playerComponent.TriedToMovePlayer += PlayerComponentOnTriedToMovePlayer;
            return _playerComponent;
        }

        public ITransformGetter GetPlayerTransformGetter() => _playerComponent;

        private void PlayerComponentOnSwitchedToNextWeapon()
        {
            var nextWeaponType = _inventoryManager.GetNextWeaponType(_currentWeapon.WeaponType());
            SetWeapon(nextWeaponType);
        }

        private void PlayerComponentOnTriedToGetAttackTarget()
        {
            if (_currentWeapon.TryGetAttackedObject(out var targetObject))
            {
                AttackedTarget?.Invoke(targetObject, _currentWeapon.DamagePerBullet);
            }
        }

        private void PlayerComponentOnTriedToMovePlayer(Vector2 moveInputValue)
        {
            if (IsMoving(moveInputValue))
            {
                PlayerMoved?.Invoke(moveInputValue);
            }
        }

        public void AddWeaponToOwnedItems(WeaponType weaponType)
        {
            var allWeaponTypesWithSlot = _playerComponent.GetWeaponTypesWithSlot();
            var weaponTypeWithSlot = allWeaponTypesWithSlot.FirstOrDefault(ws => ws.Type == weaponType);
            if (weaponTypeWithSlot != null)
            {
                _inventoryManager.AddWeaponToOwnedWeapons(weaponTypeWithSlot);
            }
            else
            {
                _inventoryManager.AddWeaponToOwnedWeapons(new WeaponTypeWithSlot(weaponType,
                    allWeaponTypesWithSlot.First().Slot));
                Debug.LogError($"There is no slot set for {weaponType.ToString()} type");
            }
        }

        public void SetWeapon(WeaponType weaponType)
        {
            if (_currentWeapon)
                _currentWeapon.UnEquip();

            _currentWeapon = _inventoryManager.GetWeapon(weaponType);
            _playerComponent.SetWeapon(weaponType, _currentWeapon.AttackRateMultiplier);
            _currentWeapon.Init(_playerComponent.gameObject);
        }

        private bool IsMoving(Vector2 moveInputValue)
        {
            return moveInputValue.sqrMagnitude > 0;
        }

        public bool IsAiming() => _playerComponent.IsAiming();

        public void OnMoveInputUpdated(Vector2 inputValue)
        {
            _playerComponent.OnMoveInputUpdated(inputValue);
        }

        public void OnAimInputUpdated(Vector2 inputValue)
        {
            _playerComponent.OnAimInputUpdated(inputValue);
        }

        public void OnAimStickTapped()
        {
            _playerComponent.StartSwitchingToNextWeapon();
        }
    }
}