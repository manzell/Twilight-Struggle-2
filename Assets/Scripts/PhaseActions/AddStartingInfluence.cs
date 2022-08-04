using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using System.Linq;
using System.Threading.Tasks;

public class AddStartingInfluence : PhaseAction 
{
    [SerializeField] Continent continent;
    [SerializeField] Faction faction;
    [SerializeField] int influence;

    public override async Task Do(Phase phase)
    {
        IEnumerable<Country> countries = Game.countries.Where(country => country.Continents.Contains(continent)); 

        Debug.Log($"Add Starting Influence - {influence} influence into {countries.Count()} Countries");
        
        if (countries.Count() > 0)
            selectionManager = new CountrySelectionManager(countries, Place);

        while (countries.Count() > 0 && influence > 0)
            await Task.Yield();

        selectionManager.CloseSelectionManager();
    }

    void Place(Country country)
    {
        country.AdjustInfluence(faction, 1);
        influence--;
    }
}
