using Scenes.Game.Framework.Creature.Containers;
using UnityEngine;

namespace Scenes.Game.Framework.Creature.Enemy.Chomper
{
    public class ChomperPresenter : Creature
    {
        private readonly ChomperComponent.Factory _factory;
        private ChomperComponent _chomperComponent;

        public ChomperPresenter(ChomperComponent.Factory factory, CreatureHealthManager healthManager) : base(
            healthManager, CreatureType.Chomper)
        {
            _factory = factory;
        }

        protected override CreatureComponent CreateComponent(Vector3 position, Transform parent)
        {
            _chomperComponent = _factory.Create(position, parent);
            return _chomperComponent;
        }
    }
}