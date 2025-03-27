using System;
using System.Collections.Generic;
using Scenes.Game.Framework.Creature.Containers;

namespace Scenes.Game.Framework.Creature.Managers
{
    public class CreaturesManager
    {
        private readonly Creature.ICreatureFactory _creatureFactory;
        private int _lastCreatureId;

        private readonly Dictionary<int, Creature> _idBasedCreatures = new();

        public CreaturesManager(Creature.ICreatureFactory creatureFactory)
        {
            _creatureFactory = creatureFactory;
        }

        public T CreateCreature<T>(CreatureType creatureType) where T : Creature
        {
            Creature creature;
            switch (creatureType)
            {
                case CreatureType.Human:
                    creature = _creatureFactory.Create<T>(_lastCreatureId, CreatureType.Human);
                    break;
                case CreatureType.Chomper:
                    creature = _creatureFactory.Create<T>(_lastCreatureId, CreatureType.Chomper);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(creatureType), creatureType, null);
            }
            creature.Died += OnCreatureDied;
            _idBasedCreatures.Add(_lastCreatureId, creature);
            _lastCreatureId++;
            return (T)creature;
        }

        private void OnCreatureDied(int creatureId) 
        {
            _idBasedCreatures[creatureId].Dispose();
            _idBasedCreatures.Remove(creatureId);
        }
    }
}