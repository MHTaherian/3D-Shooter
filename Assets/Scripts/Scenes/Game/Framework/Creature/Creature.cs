using System;
using Scenes.Game.Framework.Creature.Containers;
using Scenes.Game.Framework.Creature.Enemy.Chomper;
using Scenes.Game.Framework.Creature.Player;
using Scenes.Game.Inventory;
using UnityEngine;

namespace Scenes.Game.Framework.Creature
{
    public abstract class Creature : IDisposable
    {
        public int Id { get; private set; }

        private readonly CreatureType _type;

        public CreatureType Type => _type;

        private readonly CreatureHealthManager _healthManager;
        private CreatureComponent _creatureComponent;

        public event Action<int> Died;

        protected Creature(CreatureHealthManager healthManager, CreatureType type)
        {
            _healthManager = healthManager;
            _type = type;
        }

        public void CreateCreatureComponent(Vector3 position, Transform parent)
        {
            _creatureComponent = CreateComponent(position, parent);
            _creatureComponent.GotAttacked += CreatureComponentOnGotAttacked;
            _healthManager.OnHealthChanged += HealthManagerOnHealthChanged;
            _healthManager.CreateHealthBar(_creatureComponent.HealthBarAttachPoint);
        }

        private void CreatureComponentOnGotAttacked(float amount)
        {
            _healthManager.ChangeHealth(-amount);
        }

        private void HealthManagerOnHealthChanged(float healthChangeAmount)
        {
            var health = _healthManager.Health;
            Debug.Log(
                $"{_creatureComponent.name} health change by {healthChangeAmount} and health is {health} now");
            if (health <= 0)
            {
                _creatureComponent.Die();
                Died?.Invoke(Id);
            }
        }

        public void Dispose()
        {
            _healthManager?.Dispose();
        }

        protected abstract CreatureComponent CreateComponent(Vector3 position, Transform parent);

        public class Factory : ICreatureFactory
        {
            private readonly CreatureHealthManager.Factory _healthManagerFactory;
            private readonly PlayerComponent.Factory _playerComponentFactory;
            private readonly ChomperComponent.Factory _chomperComponentFactory;
            private readonly InventoryManager _inventoryManager;

            public Factory(CreatureHealthManager.Factory healthManagerFactory,
                PlayerComponent.Factory playerComponentFactory, ChomperComponent.Factory chomperComponentFactory, 
                InventoryManager inventoryManager)
            {
                _healthManagerFactory = healthManagerFactory;
                _playerComponentFactory = playerComponentFactory;
                _chomperComponentFactory = chomperComponentFactory;
                _inventoryManager = inventoryManager;
            }

            public T Create<T>(int id, CreatureType creatureType) where T : Creature
            {
                var healthManager = _healthManagerFactory.Create(creatureType);
                Creature creature = creatureType switch
                {
                    CreatureType.Human => new PlayerPresenter(_playerComponentFactory, _inventoryManager,
                        healthManager),
                    CreatureType.Chomper => new ChomperPresenter(_chomperComponentFactory, healthManager),
                    _ => throw new ArgumentOutOfRangeException(nameof(creatureType), creatureType, null)
                };
                creature.Id = id;
                return (T)creature;
            }
        }

        public interface ICreatureFactory
        {
            public T Create<T>(int id, CreatureType creatureType) where T : Creature;
        }
    }
}