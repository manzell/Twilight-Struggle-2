using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class CambridgeEffect : PlayerAction
{
    [SerializeField] Faction USA, USSR;
    [SerializeField] WarPhase prohibitedPhase; 

    public override bool Can(Player player, Card card) => Phase.GetCurrent<Turn>().warPhase != prohibitedPhase && base.Can(player, card);

    protected override async Task Action()
    {
        List<Continent> eligibleContinents = new();

        foreach (Card c in USA.player.hand)
            if (c.Data is ScoringCard scoringCard)
                eligibleContinents.Add(scoringCard.continent); 

        if (eligibleContinents.Count > 0)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Cambridge Five. Add 1 USSR Influence in {eligibleContinents.Implode("or")}"); 

            await new SelectionManager<Country>(Game.Countries.Where(country => country.Continents.Any(continent => eligibleContinents.Contains(continent))), 
                country => country.AdjustInfluence(USSR, 1)).Selection; 
        }
    }
}
