using Scenes.Game.Player;
using UnityEngine;
using Zenject;

namespace Scenes.Game.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponType _type;
        public GameObject Owner { get; private set; }

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