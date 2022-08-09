using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Apply Effect")]
public class ApplyEffectToCountries : PlayerAction
{
    [SerializeField] Effect effect;
    [SerializeField] Continent continent;
    [SerializeField] List<CountryData> countries;

    protected override Task Action()
    {
        foreach (CountryData countryData in continent.countries.Select(c => c.Data).Union(countries).Distinct())
            countryData.country.ongoingEffects.Add(effect);

        return Task.CompletedTask; 
    }
}
