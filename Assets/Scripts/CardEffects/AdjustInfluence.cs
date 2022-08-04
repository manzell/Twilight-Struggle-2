using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Adjust Influence")]
public class AdjustInfluence : CardEffect
{
    [SerializeField] Dictionary<CountryFaction, int> influencePlacement = new();
    [SerializeField] int influencePool;
    [SerializeField] int maxPerCountry;
    [SerializeField] List<Continent> continents;
    [SerializeField] List<Country> eligibleCountries = new();

    CountrySelectionManager selectionManager;

    class CountryFaction
    {
        public Country country;
        public Faction faction; 

        public CountryFaction(Country c, Faction f)
        {
            country = c;
            faction = f; 
        }
    }

    public override async Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        Debug.Log($"Adjust Influence {influencePool}/{maxPerCountry}");

        List<Country> clickCounter = new List<Country>();

        // First, execute any influenceChange that's hard-entered

        foreach (CountryFaction influenceKey in influencePlacement.Keys)
            influenceKey.country.AdjustInfluence(player.faction, influencePlacement[influenceKey]);

        if (influencePool != 0)
        {
            int adjustAmount = influencePool / Mathf.Abs(influencePool);

            // If we don't have a list of Specific Countries, any country from the Continents list
            if (eligibleCountries.Count == 0)
                foreach (Continent continent in continents)
                    eligibleCountries.AddRange(continent.countries);

            //If we still don't have any countries, let's add countries where we have influence as well as their neighbors
            if(eligibleCountries.Count == 0)
                foreach(Country country in Game.countries.Where(c1 => c1.Influence(player.faction) > 0 || c1.Neighbors.Any(c2 => c2.country.Influence(player.faction) > 0)))
                    eligibleCountries.Add(country);

            selectionManager = new CountrySelectionManager(eligibleCountries, OnClick);

            while (influencePool != 0)
                await Task.Yield();

            selectionManager.CloseSelectionManager();

            void OnClick(Country country)
            {
                Debug.Log("Adjust Influence Click Received");
                if (influencePool != 0 && clickCounter.Count(c => c == country) < maxPerCountry)
                {
                    country.AdjustInfluence(adjustAmount > 0 ? player.faction : player.enemyPlayer.faction, adjustAmount);
                    influencePool -= adjustAmount;
                    clickCounter.Add(country);
                }
            }
        }
    }
}
