using System.Collections.Generic;
using System.Linq;
using Scenes.Game.Weapons;
using UnityEngine;

namespace Scenes.Game.Inventory
{
    public class InventoryManager
    {
        private readonly Weapon.WeaponFactory _weaponFactory;
        private Dictionary<WeaponType, Transform> _ownedWeaponTypesWithAttachSlots = new();
        private List<Weapon> _ownedWeapons = new();

        private InventoryManager(Weapon.WeaponFactory weaponFactory)
        {
            _weaponFactory = weaponFactory;
        }

        public void AddWeaponToOwnedWeapons(WeaponType weaponType, Transform attachSlot)
        {
            _ownedWeaponTypesWithAttachSlots.Add(weaponType, attachSlot);
        }

        public Weapon GetWeapon(WeaponType weaponType)
        {
            var weapon = _ownedWeapons.FirstOrDefault(weapon => weapon.WeaponType() == weaponType);
            if (weapon != null) return weapon;
            var attachSlot = _ownedWeaponTypesWithAttachSlots[weaponType];
            weapon = _weaponFactory.Create(weaponType, attachSlot);
            _ownedWeapons.Add(weapon);
            return weapon;
        }
    }
}