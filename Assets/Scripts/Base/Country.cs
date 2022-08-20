using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector; 

public class Country : SerializedMonoBehaviour, ISelectable
{
    [SerializeField] CountryData data;
    [SerializeField] UI_Country ui;

    Dictionary<Faction, int> influence;
    public List<Effect> ongoingEffects = new();
    public List<Modifier> modifiers = new();

    public CountryData Data => data;
    public UI_Country UI => ui; 
    public List<Continent> Continents => data.continents;
    public Faction AdjacentSuperpower => data.adjacentSuperower;
    public List<CountryData> Neighbors => data.neighbors;
    public Faction Control => Mathf.Abs(Influence(Game.Players.First()) - Influence(Game.Players.Last())) >= Stability ? influence.Keys.OrderByDescending(f => influence[f]).First() : null;
    public int Stability => data.stability;
    public bool Battleground => data.battleground;
    public int Influence(Player player) => influence.TryGetValue(player.Faction, out int inf) ? inf : 0;
    public int Influence(Faction faction) => influence.TryGetValue(faction, out int inf) ? inf : 0;

    public bool Can(PlayerAction action) => ongoingEffects.All(effect => effect.Test(action));

    public void Select() { }
    public event Action onInfluencePlacementEvent;
    public event Action<ISelectable> selectionEvent
    {
        add { ui.onClickHandler += value; }
        remove { ui.onClickHandler -= value; }
    }

    void Awake()
    {
        if(data != null)
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
            onInfluencePlacementEvent?.Invoke();
        }
    }

    public void SetInfluence(Faction faction, int amount)
    {
        if(influence[faction] != amount)
        {
            Debug.Log($"Setting {faction.name} Influence in {this.name} to {amount}");
            influence[faction] = amount;
            onInfluencePlacementEvent?.Invoke();
        }
    }

    public void SetUI(UI_Country ui) => this.ui = ui; 

    public void OnSelectable() { ui.SetHighlight(Color.red); ui.Show(); }
    public void RemoveSelectable() { ui.ClearHighlight(); ui.Hide(); }
}
