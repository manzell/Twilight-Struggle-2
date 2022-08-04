using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class DestalEffect : CardEffect
{
    [SerializeField] int maxRelocate, maxPlacePerCountry; 
    [SerializeField] Faction USA; 

    CountrySelectionManager countrySelection; 
    public override async Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        int influenceRemoved = 0; 
        List<Country> eligibleCountries = Game.countries.Where(c => c.Influence(card.Faction) > 0).ToList();
        List<Country> placedCountries = new(); 

        countrySelection = new(eligibleCountries, Remove);

        while (countrySelection.finishedSelectingCountries == true || countrySelection.selectedCountries.Count() < maxRelocate)
            await Task.Yield();

        countrySelection.CloseSelectionManager();

        if(influenceRemoved > 0)
        {
            countrySelection = new(Game.countries.Where(country => country.control != USA), Place);

            while (countrySelection.finishedSelectingCountries == true || influenceRemoved == 0)
                await Task.Yield();

            countrySelection.CloseSelectionManager(); 
        }

        void Remove(Country country)
        {
            if(country.Influence(card.Faction) > 0)
            {
                country.AdjustInfluence(card.Faction, -1);
                influenceRemoved++;

                if (country.Influence(card.Faction) == 0)
                    countrySelection.RemoveSelectableCountry(country); 
            }
        }

        void Place(Country country)
        {
            if(placedCountries.Count(c => c == country) < maxPlacePerCountry)
            {
                placedCountries.Add(country); 
                country.AdjustInfluence(card.Faction, 1);
                influenceRemoved--;
            }

            if(placedCountries.Count(c => c == country) == maxPlacePerCountry)
                countrySelection.RemoveSelectableCountry(country); 
        }
    }
}
