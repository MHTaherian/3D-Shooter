using UnityEngine;
using Zenject;

namespace Scenes.Game.UI.InGameUI
{
    public interface IHealthView
    {
        public void SetAttachPoint(Transform attachPoint);

        public void SetValue(float health, float maxHealth);

        public void DestroySelf();

        public class Factory : IFactory<float, Transform, IHealthView>
        {
            private readonly DiContainer _container;
            private readonly HealthBar _healthBarPrefab;
            private readonly ITransformGetter _healthBarParent;

            public Factory(DiContainer container, HealthBar healthBarPrefab, ITransformGetter healthBarParent)
            {
                _container = container;
                _healthBarPrefab = healthBarPrefab;
                _healthBarParent = healthBarParent;
            }

            public IHealthView Create(float maxHealth, Transform attachPoint)
            {
                var healthBar =
                    _container.InstantiatePrefabForComponent<HealthBar>(_healthBarPrefab,
                        _healthBarParent.GetTransform());
                healthBar.SetValue(maxHealth, maxHealth);
                healthBar.SetAttachPoint(attachPoint);
                return healthBar;
            }
        }
    }
}