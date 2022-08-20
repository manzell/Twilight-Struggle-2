using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Place : PlayerAction
{
    public static event System.Action<InfluencePlacement> prepPlacement, placementEvent;
    public IEnumerable<Country> Placements => placements.Select(ip => ip.country); 
    List<InfluencePlacement> placements = new();
    IEnumerable<Country> EligibleCountries(Player player) =>
        Game.Countries.Where(country => country.Influence(player) > 0 || country.Neighbors.Any(neighbor => neighbor.country.Influence(player) > 0) || country.AdjacentSuperpower == player.Faction);

    public async override Task Action() 
    {
        int placedOps = 0; 
        placements = new();

        SelectionManager<Country> selectionManager = new(EligibleCountries(Player));

        while (selectionManager.open && modifiedOpsValue > placedOps)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Place {Player.name} Influence ({modifiedOpsValue - placedOps} {(modifiedOpsValue - placedOps == 1 ? "Op" : "Ops")} remaining)");

            Country country = await selectionManager.Selection;

            int placementCost = country.Control == Player.Enemy.Faction ? 2 : 1;
            int modifier = Phase.GetCurrent<Turn>().modifiers.Sum(mod => mod.Applies(this) ? mod.amount : 0);

            if (modifiedOpsValue >= placedOps + placementCost)
            {
                placements.Add(new InfluencePlacement(Player, country, placementCost));
                country.AdjustInfluence(Player.Faction, 1);

                placedOps += placementCost;
            }

            if (modifiedOpsValue - placedOps < 2)
                foreach (Country c in selectionManager.Selected.Where(c1 => c1.Control == Player.Enemy))
                    selectionManager.RemoveSelectable(c);
        }

        selectionManager.Close();
    }

    public override bool Can(Player player, Card card) => card.Data is not ScoringCard && EligibleCountries(player).Count() > 0;

    public class InfluencePlacement
    {
        public Player player;
        public Country country;
        public int influenceCost { get; private set; }

        public InfluencePlacement(Player player, Country country, int cost)
        {
            prepPlacement?.Invoke(this);
            this.player = player;
            this.country = country;
            influenceCost = cost;
            placementEvent?.Invoke(this);
        }
    }
}
