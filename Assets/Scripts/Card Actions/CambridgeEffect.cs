using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class CambridgeEffect : PlayerAction
{
    [SerializeField] Faction USA, USSR;
    [SerializeField] WarPhase prohibitedPhase; 

    public override bool Can(Player player, Card card)
    {
        if (Phase.GetCurrent<Turn>().warPhase == prohibitedPhase) return false; 
        return base.Can(player, card);
    }

    public string Implode<T>(IEnumerable<T> someList, string etc = "and", string divisor = ",")
    {
        string ret = string.Empty;
        int listLength = someList.Count(); 

        for(int i = 0; i < listLength; i++)
        {
            ret += someList.ElementAt(i); 
            
            if (i + 2 == listLength)
                ret += $" {etc} ";

            if (i + 2 < listLength)
                ret += $"{divisor} "; 
        }

        return ret; 
    }

    protected override async Task Action(Player player, Card card)
    {
        List<Continent> eligibleContinents = new();

        foreach (Card c in USA.player.hand)
            if (c.Data is ScoringCard scoringCard)
                eligibleContinents.Add(scoringCard.continent); 

        if (eligibleContinents.Count > 0)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Cambridge Five. Add 1 USSR Influence in {Implode(eligibleContinents)}"); 

            await new SelectionManager<Country>(Game.Countries.Where(country => country.Continents.Any(continent => eligibleContinents.Contains(continent))), 
                country => country.AdjustInfluence(USSR, 1)).Selection; 
        }
    }
}
