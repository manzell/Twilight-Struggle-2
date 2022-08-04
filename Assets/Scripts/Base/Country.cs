using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; 

public class Country : MonoBehaviour
{
    [SerializeField] CountryData data;
    [SerializeField] Dictionary<Faction, int> influence = new();

    public List<Effect> ongoingEffects = new();
    public List<Faction> factions => influence.Keys.ToList();

    public UI_Country ui; 

    public static event Action<Country> OnInfluencePlacementEvent;
    public event Action onInfluencePlacementEvent;

    public CountryData Data => data;
    public int Stability => data.stability;
    public bool Battleground => data.battleground;
    public List<Continent> Continents => data.continents;
    public Faction AdjacentSuperower => data.adjacentSuperower;
    public List<CountryData> Neighbors => data.neighbors;

    public bool Can(GameAction action) => ongoingEffects.All(effect => effect.Test(action));

    public int Influence(Player player) => influence.TryGetValue(player.faction, out int inf) ? inf : 0;
    public int Influence(Faction faction) => influence.TryGetValue(faction, out int inf) ? inf : 0;

    void Awake()
    {
        if(data != null)
        {
            influence = new(data.startingInfluence);
            data.country = this;
        }
    }

    public Faction control
    {
        get
        {
            List<Faction> keys = influence.Keys.ToList();

            if (keys.Count == 1 && influence[keys[0]] >= Stability)
                return keys[0]; 
            if(keys.Count == 2)
            {
                if (influence[keys[0]] - influence[keys[1]] >= Stability)
                    return keys[0];
                if (influence[keys[1]] - influence[keys[0]] >= Stability)
                    return keys[1];
            }

            return null;
        }
    }

    public void AdjustInfluence(Faction faction, int amount)
    {
        influence[faction] = Mathf.Max(0, influence[faction] + amount);
        OnInfluencePlacementEvent?.Invoke(this);
        onInfluencePlacementEvent?.Invoke();
    }

    public void SetInfluence(Faction faction, int amount)
    {
        influence[faction] = amount;
        OnInfluencePlacementEvent?.Invoke(this);
        onInfluencePlacementEvent?.Invoke();
    }
}
