using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class Place : PlayerAction
{
    public static event System.Action<Place> prepPlacement, placementEvent, endPlacement;
    public IEnumerable<Country> PlacedCountries => placements.Select(ip => ip.country); 

    List<InfluencePlacement> placements = new();

    protected override Task Action(Player player, Card card) => Event(card.ops, player);
    async Task Event(int Ops, Player player)
    {
        int placementOps = Ops;
        bool removedEnemyControlled = false; 
        prepPlacement?.Invoke(this);

        SelectionManager<Country> selectionManager = 
            new(Game.Countries.Where(country => country.Influence(player) > 0 || country.Neighbors.Any(neighbor => neighbor.country.Influence(player) > 0) || country.AdjacentSuperpower == player.faction), 
            DoPlace);

        while (selectionManager.open && placementOps + modifier > 0)
        {
            if (!removedEnemyControlled && placementOps + modifier == 1)
            {
                removedEnemyControlled = true; 
                foreach (Country country in selectionManager.Selected.Where(country => country.Control == player.enemyPlayer))
                    selectionManager.RemoveSelectable(country);
            }

            await selectionManager.Selection;
        }

        endPlacement?.Invoke(this); 

        void DoPlace(Country country)
        {
            int placementCost = country.Control == player.enemyPlayer.faction ? 2 : 1;
            int modifier = Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);

            if (placementOps + modifier >= placementCost)
            {
                placementOps -= placementCost;
                placements.Add(new InfluencePlacement(player, country, placementCost));
                country.AdjustInfluence(player.faction, 1);
                placementEvent?.Invoke(this);
            }
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
