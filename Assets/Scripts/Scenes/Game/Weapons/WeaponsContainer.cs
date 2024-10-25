using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Game.Weapons
{
    [CreateAssetMenu(menuName = "Containers/Create Weapons Container", fileName = "WeaponsContainer")]
    public class WeaponsContainer : ScriptableObject
    {
        [SerializeField] private Weapon _defaultWeaponPrefab;
        [SerializeField] private Weapon[] _weaponPrefabs;

        public Weapon GetWeaponPrefab(WeaponType type)
        {
            return _weaponPrefabs.FirstOrDefault(weapon => weapon.WeaponType() == type) ?? _defaultWeaponPrefab;
        }
    }
}