using System;
using UnityEngine;

namespace Scenes.Game.Framework.Creature
{
    public abstract class CreatureComponent : MonoBehaviour
    {
        [Header("Animation")] [SerializeField]
        protected Animator Animator;

        private static readonly int Dead = Animator.StringToHash("Dead");
        public event Action<float> GotAttacked; 
        public void OnGotAttacked(float damagePerBullet)
        {
            GotAttacked?.Invoke(damagePerBullet);
        }

        public void ChangeHealth(float amount)
        {
            
        }

        public void Die()
        {
            TriggerDeathAnimation();
        }

        private void TriggerDeathAnimation()
        {
            Animator?.SetTrigger(Dead);
        }
        
        //Animator event function
        public void OnDeathAnimationFinished()
        {
            Destroy(gameObject);
        }
    }
}