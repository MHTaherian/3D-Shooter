using System;
using Scenes.Game.Framework.Creature.Containers;
using Scenes.Game.UI.InGameUI;
using UnityEngine;
using Zenject;

namespace Scenes.Game.Framework.Creature
{
    public class CreatureHealthManager : IDisposable
    {
        private float _health;
        private readonly float _maxHealth;
        private readonly IHealthView.Factory _healthViewFactory;
        private IHealthView _healthView;

        public float Health => _health;
        public float MaxHealth => _maxHealth;

        public event HealthChangeDelegate OnHealthChanged;

        private CreatureHealthManager(IHealthView.Factory healthViewFactory, float maxHealth)
        {
            _healthViewFactory = healthViewFactory;
            _health = _maxHealth = maxHealth;
        }

        public void CreateHealthBar(Transform attachPoint)
        {
            _healthView = _healthViewFactory.Create(_maxHealth, attachPoint);
        }

        public void ChangeHealth(float amount)
        {
            if (amount == 0 || _health <= 0)
                return;
            if (_maxHealth >= _health + amount)
            {
                if (_health + amount <= 0)
                {
                    amount = -_health;
                }
                _health += amount;
            }
            else
            {
                amount = _maxHealth - _health;
                _health = _maxHealth;
            }
            _healthView?.SetValue(_health, _maxHealth);
            OnHealthChanged?.Invoke(amount);
        }

        public void Dispose()
        {
            _healthView?.DestroySelf();
        }

        public class Factory : IFactory<CreatureType, CreatureHealthManager>
        {
            private readonly CreaturesDataContainer _creaturesDataContainer;
            private readonly IHealthView.Factory _healthViewFactory;

            public Factory(CreaturesDataContainer creaturesDataContainer, IHealthView.Factory healthViewFactory)
            {
                _creaturesDataContainer = creaturesDataContainer;
                _healthViewFactory = healthViewFactory;
            }
            public CreatureHealthManager Create(CreatureType creatureType)
            {
                var creatureData = _creaturesDataContainer.GetCreatureData(creatureType);
                return new CreatureHealthManager(_healthViewFactory, creatureData.MaxHealth);
            }
        }
    }

    public delegate void HealthChangeDelegate(float healthChangeAmount);
}