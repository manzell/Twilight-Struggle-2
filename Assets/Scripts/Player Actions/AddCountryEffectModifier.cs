using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq; 

public class AddCountryEffectModifier : PlayerAction
{
    [SerializeField] Modifier mod;
    [SerializeField] Effect effect;
    [SerializeField] List<CountryData> eligibleCountries = new();

    public async override Task Action()
    {
        if(eligibleCountries.Count > 0)
        {
            SelectionManager<Country> selection = new(eligibleCountries.Select(data => data.country), 1);
            Country country = await selection.Selection; 
            AddModifier(country);
        }
    }

    void AddModifier(Country country)
    {
        if (mod != null)
            country.modifiers.Add(mod);
        if (effect != null)
            country.ongoingEffects.Add(effect);
    }
}
