using System;
using UnityEngine;

namespace Scenes.Game.Framework.Creature
{
    public abstract class CreatureComponent : MonoBehaviour
    {
        public event Action<float> GotAttacked; 
        public void OnGotAttacked(float damagePerBullet)
        {
            GotAttacked?.Invoke(damagePerBullet);
        }

        public void ChangeHealth(float amount)
        {
            
        }
    }
}