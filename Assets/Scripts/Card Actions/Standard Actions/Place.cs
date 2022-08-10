using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class Place : PlayerAction
{
    public static event System.Action<Place> prepPlacement, placementEvent, endPlacement;
    public IEnumerable<Country> Placements => placements.Select(ip => ip.country); 
    List<InfluencePlacement> placements = new();
    IEnumerable<Country> EligibleCountries(Player player) =>
        Game.Countries.Where(country => country.Influence(player) > 0 || country.Neighbors.Any(neighbor => neighbor.country.Influence(player) > 0) || country.AdjacentSuperpower == player.faction);

    protected async override Task Action() 
    {
        int placedOps = 0; 
        placements = new();

        prepPlacement?.Invoke(this);

        SelectionManager<Country> selectionManager = 
            new(EligibleCountries(Player), DoPlace);

        while (selectionManager.open && modifiedOpsValue > placedOps)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Place {Player.name} Influence ({modifiedOpsValue - placedOps} {(modifiedOpsValue - placedOps == 1 ? "Op" : "Ops")} remaining)");
                
            if (modifiedOpsValue - placedOps < 2)
                foreach (Country country in selectionManager.Selected.Where(country => country.Control == Player.enemyPlayer))
                    selectionManager.RemoveSelectable(country);

            await selectionManager.Selection;
        }

        selectionManager.Close(); 
        endPlacement?.Invoke(this); 

        void DoPlace(Country country)
        {
            int placementCost = country.Control == Player.enemyPlayer.faction ? 2 : 1;
            int modifier = Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);

            if (modifiedOpsValue >= placedOps + placementCost)
            {
                placements.Add(new InfluencePlacement(Player, country, placementCost));
                country.AdjustInfluence(Player.faction, 1);

                placedOps += placementCost;
                placementEvent?.Invoke(this);
            }
        }
    }

    public override bool Can(Player player, Card card) => card.Data is not ScoringCard && EligibleCountries(player).Count() > 0;

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
