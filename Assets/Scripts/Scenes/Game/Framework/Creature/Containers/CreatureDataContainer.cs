using System;
using System.Linq;
using UnityEngine;

namespace Scenes.Game.Framework.Creature.Containers
{
    [CreateAssetMenu(menuName = "Containers/Create Creatures Data Container", fileName = "CreaturesDataContainer")]
    public class CreaturesDataContainer : ScriptableObject
    {
        [SerializeField] private CreatureData[] CreaturesData;
        [SerializeField] private CreatureData DefaultCreatureData;

        public CreatureData GetCreatureData(CreatureType creatureType)
        {
            var data = CreaturesData.Any(d => d.CreatureType == creatureType)
                ? CreaturesData.First(d => d.CreatureType == creatureType)
                : DefaultCreatureData;
            return data;
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