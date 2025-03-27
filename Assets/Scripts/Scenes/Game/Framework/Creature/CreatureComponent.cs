using System;
using Scenes.Game.UI.InGameUI;
using UnityEngine;

namespace Scenes.Game.Framework.Creature
{
    public abstract class CreatureComponent : MonoBehaviour
    {
        [Header("Animation")] [SerializeField]
        protected Animator Animator;

        [Space(16)][SerializeField] private Transform _healthBarAttachPoint;

        public Transform HealthBarAttachPoint => _healthBarAttachPoint;

        private static readonly int Dead = Animator.StringToHash("Dead");

        public event Action<float> GotAttacked;

        public void OnGotAttacked(float damagePerBullet)
        {
            GotAttacked?.Invoke(damagePerBullet);
        }

        public void Die(bool immediately = false)
        {
            if (immediately)
                Destroy(gameObject);
            else
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