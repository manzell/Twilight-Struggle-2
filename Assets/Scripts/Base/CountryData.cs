using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System; 
using System.Linq;
using Sirenix.OdinInspector;

namespace TwilightStruggle
{
    [CreateAssetMenu]
    public class CountryData : SerializedScriptableObject
    {
        public int stability;
        public bool isBattleground;
        public List<Continent> continents;
        public Faction adjacentSuperower;
        public List<CountryData> neighbors;
        public Dictionary<Faction, int> startingInfluence = new Dictionary<Faction, int>();
        public Country country;

        private void OnDisable()
        {
            country = null;
        }
    }
}