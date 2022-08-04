using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

[CreateAssetMenu]
public class Place : GameAction, ITargetCountries
{
    public static event System.Action<Place> prepPlacement, placementEvent, endPlacement; 

    public List<Country> TargetCountries => placements.Select(p => p.country).ToList(); 
    List<InfluencePlacement> placements = new();
    int placementOps = 0;

    public override Task Event(Player player, Card card) => Event(card.ops, player);
    public async Task Event(int Ops, Player player)
    {
        this.player = player;
        placementOps = Ops;

        prepPlacement?.Invoke(this);

        selectionManager = new CountrySelectionManager(
            Game.countries.Where(country => country.Influence(player) > 0 || country.Neighbors.Any(neighbor => neighbor.country.Influence(player) > 0) || country.AdjacentSuperower == player.faction), 
            DoPlace);

        while (placementOps > 0)
            await Task.Yield();

        selectionManager.CloseSelectionManager(); 
    }

    void DoPlace(Country country)
    {
        int placementCost = country.control == player.enemyPlayer.faction ? 2 : 1;
        int modifier = Game.currentPhase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);

        if (placementOps + modifier >= placementCost)
        {
            placementOps -= placementCost;
            placements.Add(new InfluencePlacement(player, country, placementCost));
            country.AdjustInfluence(player.faction, 1);
            placementEvent?.Invoke(this);
        }

        if (placementOps + modifier == 1)
            selectionManager.RemoveSelectableCountries(selectionManager.SelectableCountries.Where(country => country.control == player.enemyPlayer).ToList());
        if (placementOps + modifier == 0)
        {
            selectionManager.CloseSelectionManager();
            endPlacement?.Invoke(this);
        }
    }

    public class InfluencePlacement
    {
        public Player player;
        public Country country;
        public int influenceCost { get; private set; }

        public InfluencePlacement(Player player, Country country, int cost)
        {
            this.player = player;
            this.country = country;
            influenceCost = cost;
        }
    }
}
