using UnityEngine;
using Zenject;

namespace Scenes.Game.Framework.Creature.Enemy.Chomper
{
    public class ChomperComponent : CreatureComponent
    {
        #region [Factory]

        public class Factory : IFactory<Vector3, Transform, ChomperComponent>
        {
            private readonly DiContainer _container;
            private readonly ChomperComponent _chomperComponentPrefab;

            public Factory(DiContainer container, ChomperComponent chomperComponentPrefab)
            {
                _container = container;
                _chomperComponentPrefab = chomperComponentPrefab;
            }

            public ChomperComponent Create(Vector3 position, Transform parent)
            {
                return _container.InstantiatePrefabForComponent<ChomperComponent>(_chomperComponentPrefab, position,
                    Quaternion.identity, parent);
            }
        }

        #endregion
    }
}