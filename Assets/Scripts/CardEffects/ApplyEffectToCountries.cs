using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Apply Effect")]
public class ApplyEffectToCountries : CardEffect
{
    [SerializeField] Effect effect;
    [SerializeField] Continent continent;
    [SerializeField] List<Country> countries = new(); 

    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        foreach (Country country in continent.countries.Union(countries).Distinct())
            country.ongoingEffects.Add(effect);

        return Task.CompletedTask; 
    }
}
