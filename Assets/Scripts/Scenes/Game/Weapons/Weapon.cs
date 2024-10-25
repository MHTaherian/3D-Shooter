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
        
        public class WeaponFactory : IFactory<WeaponType, Transform, Weapon>
        {
            private readonly DiContainer _container;
            private readonly WeaponsContainer _weaponsContainer;

            private WeaponFactory(DiContainer container, WeaponsContainer weaponsContainer)
            {
                _container = container;
                _weaponsContainer = weaponsContainer;
            }
            
            public Weapon Create(WeaponType weaponType, Transform parent)
            {
                var weaponPrefab = _weaponsContainer.GetWeaponPrefab(weaponType);
                return _container.InstantiatePrefabForComponent<Weapon>(weaponPrefab.gameObject, parent);
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
