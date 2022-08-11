using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class MatchOpponentInfluence : PlayerAction
{
    public List<CountryData> eligibleCountries = new();

    protected override async Task Action()
    {
        Faction faction = Card.Faction ?? Player.Faction;
        eligibleCountries = eligibleCountries.Where(c => c.country.Influence(faction) < c.country.Influence(faction.enemyFaction)).ToList();

        if (eligibleCountries.Count > 0)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Select a country to match {Player.Enemy.name} influence");

            SelectionManager<Country> selectionManager = new (eligibleCountries.Select(c => c.country), country => {
                country.SetInfluence(faction, country.Influence(faction.enemyFaction));
            });

            await selectionManager.Selection;
            selectionManager.Close(); 
        }
    }
}
