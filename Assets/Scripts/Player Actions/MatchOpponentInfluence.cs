using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class MatchOpponentInfluence : PlayerAction
{
    public List<CountryData> eligibleCountries = new();

    public override async Task Action()
    {
        eligibleCountries = eligibleCountries.Where(c => c.country.Influence(Player.Faction) < c.country.Influence(Player.Enemy.Faction)).ToList();

        if (eligibleCountries.Count > 0)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Select a country to match {Player.Enemy.name} influence");

            SelectionManager<Country> selectionManager = new (eligibleCountries.Select(c => c.country));

            Country country = await selectionManager.Selection as Country; 
            country.SetInfluence(Player.Faction, country.Influence(Player.Enemy.Faction));

            selectionManager.Close(); 
        }
    }
}
