using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class TrumanEffect : CardEffect
{
    [SerializeField] Continent europe; 

    public override async Task Event(Card card, Player player)
    {
        IEnumerable<Country> eligibleCountries = Game.countries.Where(c => c.Continents.Contains(europe) && c.control == null); 

        if(eligibleCountries.Count() > 0)
        {
            CountrySelectionManager selection = new(eligibleCountries,
                country => country.SetInfluence(player.enemyPlayer.faction, 0));

            while (selection.selectedCountries.Count == 0)
                await Task.Yield(); 
        }
    }
}
