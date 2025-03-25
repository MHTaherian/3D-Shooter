using UnityEditor.Experimental;
using UnityEngine;

namespace Scenes.Game.Framework.Creature
{
    public class CreatureHealthManager
    {
        private float _health;
        private readonly float _maxHealth;

        public float Health => _health;
        public float MaxHealth => _maxHealth;

        public event HealthChangeDelegate OnHealthChanged;

        public CreatureHealthManager(float maxHealth)
        {
            _health = _maxHealth = maxHealth;
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
            OnHealthChanged?.Invoke(amount);
        }
    }

    public delegate void HealthChangeDelegate(float healthChangeAmount);
}