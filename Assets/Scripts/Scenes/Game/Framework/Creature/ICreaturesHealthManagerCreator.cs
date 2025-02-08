using Scenes.Game.Framework.Creature.Containers;

namespace Scenes.Game.Framework.Creature
{
    public interface ICreaturesHealthManagerCreator
    {
        public CreatureHealthManager CreateHealthManager(CreatureType creatureType);
    }
}