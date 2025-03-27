using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scenes.Game.UI.InGameUI
{
    public class HealthBar : MonoBehaviour , IHealthView
    {
        [SerializeField] private Slider _healthSlider;
        private Transform _attachPoint;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void SetAttachPoint(Transform attachPoint)
        {
            _attachPoint = attachPoint;
        }
        
        public void SetValue(float health, float maxHealth)
        {
            _healthSlider.value = health / maxHealth;
        }

        private void Update()
        {
            if (!_attachPoint) return;
            var attachPointScreenPoint = _camera.WorldToScreenPoint(_attachPoint.position);
            transform.position = attachPointScreenPoint;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}