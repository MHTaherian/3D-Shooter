using System;
using Scenes.Game.Framework.Creature.Player;
using UnityEngine;
using Zenject;

namespace Scenes.Game.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponType _type;
        [SerializeField] private float _damagePerBullet;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private float _maxRange = 1000;
        [SerializeField] private LayerMask _enemyMask;
        [SerializeField] private float _attackRateMultiplier = 1;

        public float AttackRateMultiplier => _attackRateMultiplier;

        protected TargetingManager _targetingManager;
        public GameObject Owner { get; private set; }

        private void Awake()
        {
            _targetingManager = new TargetingManager(_muzzle, _maxRange, _enemyMask);
        }

        public void Init(GameObject owner)
        {
            Owner = owner;
            Equip();
        }

        public WeaponType WeaponType() => _type;

        public void Equip()
        {
            gameObject.SetActive(true);
        }

        public void UnEquip()
        {
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            var muzzlePosition = _muzzle.position;
            Gizmos.DrawLine(muzzlePosition, muzzlePosition + _targetingManager.GetAimDir() * _maxRange);
        }

        public bool TryGetAttackedObject(out GameObject targetObject)
        {
            var shotATarget = _targetingManager.TryGetTargetObject(out targetObject);
            return shotATarget;
        }

        public float DamagePerBullet => _damagePerBullet;

        public class WeaponFactory : IFactory<WeaponTypeWithSlot, Weapon>
        {
            private readonly DiContainer _container;
            private readonly WeaponsContainer _weaponsContainer;

            private WeaponFactory(DiContainer container, WeaponsContainer weaponsContainer)
            {
                _container = container;
                _weaponsContainer = weaponsContainer;
            }

            public Weapon Create(WeaponTypeWithSlot weaponTypeWithSlot)
            {
                var weaponPrefab = _weaponsContainer.GetWeaponPrefab(weaponTypeWithSlot.Type);
                return _container.InstantiatePrefabForComponent<Weapon>(weaponPrefab.gameObject,
                    weaponTypeWithSlot.Slot);
            }
        }
    }

    public enum WeaponType
    {
        None,
        Pistol,
        Rifle
    }
}