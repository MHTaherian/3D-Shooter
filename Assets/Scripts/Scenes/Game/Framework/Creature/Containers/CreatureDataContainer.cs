using System;
using System.Linq;
using UnityEngine;

namespace Scenes.Game.Framework.Creature.Containers
{
    [CreateAssetMenu(menuName = "Containers/Create Creatures Data Container", fileName = "CreaturesDataContainer")]
    public class CreaturesDataContainer : ScriptableObject, ICreaturesHealthManagerCreator
    {
        [SerializeField] private CreatureData[] CreaturesData;
        [SerializeField] private CreatureData DefaultCreatureData;

        private CreatureData GetCreatureData(CreatureType creatureType)
        {
            var data = CreaturesData.Any(d => d.CreatureType == creatureType)
                ? CreaturesData.First(d => d.CreatureType == creatureType)
                : DefaultCreatureData;
            return data;
        }

        public CreatureHealthManager CreateHealthManager(CreatureType creatureType)
        {
            var data = GetCreatureData(creatureType);

            return new CreatureHealthManager(data.MaxHealth);
        }
    }

    [Serializable]
    public struct CreatureData
    {
        public CreatureType CreatureType;
        public int MaxHealth;
    }

    public enum CreatureType
    {
        None,
        Human,
        Chomper
    }
}