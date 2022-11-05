using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;

namespace TwilightStruggle
{
    public class Country : SerializedMonoBehaviour, ISelectable
    {
        [SerializeField] CountryData data;
        public event Action<ISelectable> selectionEvent;
        public event Action<int> influenceChangeEvent; 

        [SerializeField] Dictionary<Faction, int> influence;
        public List<Effect> ongoingEffects { get; private set; } = new();
        public List<Modifier> modifiers { get; private set; } = new();

        public CountryData Data => data;
        public List<Continent> Continents => data.continents;
        public Faction AdjacentSuperpower => data.adjacentSuperower;
        public List<CountryData> Neighbors => data.neighbors;
        public Faction Control => Mathf.Abs(Influence(Game.Players.First()) - Influence(Game.Players.Last())) >= Stability ? 
            influence.Keys.OrderByDescending(faction => influence[faction]).First() : null;
        public int Stability => data.stability;
        public bool IsBattleground => data.isBattleground;
        public int Influence(Player player) => influence.TryGetValue(player.Faction, out int inf) ? inf : 0;
        public int Influence(Faction faction) => influence.TryGetValue(faction, out int inf) ? inf : 0;

        public bool Can(PlayerAction action) => ongoingEffects.All(effect => effect.Test(action));

        public void Select() => selectionEvent.Invoke(this); 

        void Awake()
        {
            if (data != null)
            {
                influence = new(data.startingInfluence);
                data.country = this;
            }
        }

        public void AdjustInfluence(Faction faction, int amount)
        {
            amount = Mathf.Max(0, influence[faction] + amount) - influence[faction];

            if (amount != 0)
            {
                Debug.Log($"{(amount > 0 ? "Adding" : "Removing")} {Mathf.Abs(amount)} {faction.name} Influence in {this.name}");
                influence[faction] += amount;
                influenceChangeEvent.Invoke(amount);
            }
        }

        public void SetInfluence(Faction faction, int amount)
        {
            if (influence[faction] != amount)
            {
                Debug.Log($"Setting {faction.name} Influence in {this.name} to {amount}");
                influence[faction] = amount;
                influenceChangeEvent.Invoke(amount - influence[faction]);
            }
        }
    }
}