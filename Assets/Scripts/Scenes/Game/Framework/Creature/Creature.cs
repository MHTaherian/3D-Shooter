using System;
using Scenes.Game.Framework.Creature.Containers;
using Scenes.Game.Framework.Creature.Enemy.Chomper;
using Scenes.Game.Framework.Creature.Player;
using Scenes.Game.Inventory;
using UnityEngine;
using Zenject;

namespace Scenes.Game.Framework.Creature
{
    public abstract class Creature
    {
        private readonly CreatureHealthManager _healthManager;
        public CreatureComponent CreatureComponent;

        protected Creature(CreatureHealthManager healthManager)
        {
            _healthManager = healthManager;
        }

        public void CreateCreatureComponent(Vector3 position, Transform parent)
        {
            CreatureComponent = CreateComponent(position, parent);
            CreatureComponent.GotAttacked += CreatureComponentOnGotAttacked;
            _healthManager.OnHealthChanged += HealthManagerOnOnHealthChanged;
        }

        private void CreatureComponentOnGotAttacked(float amount)
        {
            _healthManager.ChangeHealth(-amount);
        }

        private void HealthManagerOnOnHealthChanged(float healthChangeAmount)
        {
            CreatureComponent.ChangeHealth(healthChangeAmount);
            Debug.Log(
                $"{CreatureComponent.name} health change by {healthChangeAmount} and health is {_healthManager.Health} now");
            if (_healthManager.Health < 0)
            {
            }
        }

        protected abstract CreatureComponent CreateComponent(Vector3 position, Transform parent);

        public class Factory : ICreatureFactory
        {
            private readonly HealthSystem _healthSystem;
            private readonly PlayerComponent.Factory _playerComponentFactory;
            private readonly ChomperComponent.Factory _chomperComponentFactory;
            private readonly InventoryManager _inventoryManager;

            public Factory(HealthSystem healthSystem, PlayerComponent.Factory playerComponentFactory,
                ChomperComponent.Factory chomperComponentFactory, InventoryManager inventoryManager)
            {
                _healthSystem = healthSystem;
                _playerComponentFactory = playerComponentFactory;
                _chomperComponentFactory = chomperComponentFactory;
                _inventoryManager = inventoryManager;
            }

            public T Create<T>(CreatureType creatureType) where T : Creature
            {
                var healthManager = _healthSystem.CreateHealthManagerForCreature(creatureType);
                
                return creatureType switch
                {
                    CreatureType.Human => new PlayerPresenter(_playerComponentFactory, _inventoryManager, healthManager)
                        as T,
                    CreatureType.Chomper => new ChomperPresenter(_chomperComponentFactory, healthManager) as T,
                    _ => throw new ArgumentOutOfRangeException(nameof(creatureType), creatureType, null)
                };
            }
        }

        public interface ICreatureFactory
        {
            public T Create<T>(CreatureType creatureType) where T : Creature;
        }
    }
}