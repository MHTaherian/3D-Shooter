using System;
using System.Collections.Generic;
using System.Linq;
using Scenes.Game.Player;
using Scenes.Game.Weapons;
using UnityEngine;

namespace Scenes.Game.Inventory
{
    public class InventoryManager
    {
        private readonly Weapon.WeaponFactory _weaponFactory;
        private List<WeaponTypeWithSlot> _ownedWeaponTypesWithAttachSlots = new();
        private List<Weapon> _ownedWeapons = new();

        private InventoryManager(Weapon.WeaponFactory weaponFactory)
        {
            _weaponFactory = weaponFactory;
        }

        public void AddWeaponToOwnedWeapons(WeaponTypeWithSlot weaponTypeWithSlot)
        {
            _ownedWeaponTypesWithAttachSlots.Add(weaponTypeWithSlot);
        }

        public Weapon GetWeapon(WeaponType weaponType)
        {
            var weapon = _ownedWeapons.FirstOrDefault(weapon => weapon.WeaponType() == weaponType);
            if (weapon != null) return weapon;
            var ownedWeaponWithSlot = _ownedWeaponTypesWithAttachSlots.FirstOrDefault(ws => ws.Type == weaponType);
            if (ownedWeaponWithSlot != null)
            {
                weapon = _weaponFactory.Create(ownedWeaponWithSlot);
            }

            _ownedWeapons.Add(weapon);
            return weapon;
        }

        public WeaponType GetNextWeaponType(WeaponType currentWeaponType)
        {
            var currentWeaponTypeWithSlotIndex =
                _ownedWeaponTypesWithAttachSlots.FindIndex(ws => ws.Type == currentWeaponType);
            var nextWeaponIndex = currentWeaponTypeWithSlotIndex < _ownedWeaponTypesWithAttachSlots.Count - 1
                ? currentWeaponTypeWithSlotIndex + 1
                : 0;
            var nextWeaponType = _ownedWeaponTypesWithAttachSlots[nextWeaponIndex].Type;
            return nextWeaponType;
        }
    }
}