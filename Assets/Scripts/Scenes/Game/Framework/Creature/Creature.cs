using System;
using Scenes.Game.Framework.Creature.Containers;
using Scenes.Game.Framework.Creature.Player;
using Scenes.Game.Inventory;
using UnityEngine;
using Zenject;

namespace Scenes.Game.Framework.Creature
{
    public abstract class Creature
    {
        protected readonly CreatureHealthManager HealthManager;

        protected Creature(CreatureHealthManager healthManager)
        {
            HealthManager = healthManager;
        }

        public abstract void CreateComponent(Vector3 position, Transform parent);

        public class Factory : ICreatureFactory
        {
            private readonly HealthSystem _healthSystem;
            private readonly PlayerComponent.Factory _playerComponentFactory;
            private readonly InventoryManager _inventoryManager;

            public Factory(HealthSystem healthSystem, PlayerComponent.Factory playerComponentFactory, InventoryManager inventoryManager)
            {
                _healthSystem = healthSystem;
                _playerComponentFactory = playerComponentFactory;
                _inventoryManager = inventoryManager;
            }
            public T Create<T>(CreatureType creatureType) where T : Creature
            {
                var healthManager = _healthSystem.CreateHealthManagerForCreature(creatureType);
                return creatureType switch
                {
                    CreatureType.Human => new PlayerPresenter(_playerComponentFactory, _inventoryManager, healthManager)
                        as T,
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