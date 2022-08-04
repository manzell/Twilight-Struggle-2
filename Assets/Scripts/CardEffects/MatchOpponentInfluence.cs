using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class MatchOpponentInfluence : CardEffect
{
    public List<Country> eligibleCountries = new();

    public override async Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        Faction faction = card.Faction ?? player.faction;
        eligibleCountries = eligibleCountries.Where(country => country.Influence(faction) < country.Influence(faction.enemyFaction)).ToList();

        if (eligibleCountries.Count == 1)
            Match(eligibleCountries.First());
        else if (eligibleCountries.Count > 1)
        {
            CountrySelectionManager selection = new CountrySelectionManager(eligibleCountries, Match);

            while (selection.selectedCountries.Count < 1)
                await Task.Yield();

            selection.CloseSelectionManager(); 
        }
        else Match(null); 

        void Match(Country country) => country?.SetInfluence(faction, country.Influence(faction.enemyFaction));
    }
}
