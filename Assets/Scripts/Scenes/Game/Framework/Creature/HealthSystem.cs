using Scenes.Game.Framework.Creature.Containers;

namespace Scenes.Game.Framework.Creature
{
    public class HealthSystem
    {
        private readonly ICreaturesHealthManagerCreator _healthManagerCreator;

        public HealthSystem(ICreaturesHealthManagerCreator healthManagerCreator)
        {
            _healthManagerCreator = healthManagerCreator;
        }

        public CreatureHealthManager CreateHealthManagerForCreature(CreatureType creatureType)
        {
            return _healthManagerCreator.CreateHealthManager(creatureType);
        }
    }
}