using UnityEngine;

namespace Scenes.Game.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public GameObject Owner { get; private set; }

        public void Init(GameObject owner)
        {
            Owner = owner;
        }

        public void Equip()
        {
            gameObject.SetActive(true);
        }

        public void UnEquip()
        {
            gameObject.SetActive(false);
        }
    }
}
