using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class SpecialRelationship : CardEffect
{
    [SerializeField] Country UK;
    [SerializeField] Player US; 
    [SerializeField] Effect NATO;
    [SerializeField] Continent WesternEurope; 

    public override async Task Event(Card card, Player player)
    {
        Debug.Log($"Event for {card.name} received");
        if (UK.control == US.faction)
        {
            if(Game.activeEffects.Contains(NATO))
            {
                Game.AdjustVP(US, 2);
                
                CountrySelectionManager select = new(WesternEurope.countries, country => country.AdjustInfluence(US.faction, 2));

                while (select.selectedCountries.Count != 1)
                    await Task.Yield(); 
            }
            else // Add 1 Influence to any country adjacent to the UK
            {
                CountrySelectionManager select = new(UK.Neighbors.Select(n => n.country), country => country.AdjustInfluence(US.faction, 1));

                while (select.selectedCountries.Count != 1)
                    await Task.Yield();
            }
        }
    }
}
