using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class AddBonusInfluence : PhaseAction
{
    [SerializeField] Faction faction;
    [SerializeField] int influence;

    public async override Task Do(Phase phase)
    {
        IEnumerable<Country> countries = Game.countries.Where(country => country.Influence(faction) > 0);

        if (countries.Count() > 0)
            selectionManager = new CountrySelectionManager(countries, PlaceInfluence);
        
        while (countries.Count() > 0 && influence > 0)
            await Task.Yield(); 

        selectionManager.CloseSelectionManager(); 
    }

    private void PlaceInfluence(Country country)
    {
        country.AdjustInfluence(faction, 1);
        influence--;
    }
}
