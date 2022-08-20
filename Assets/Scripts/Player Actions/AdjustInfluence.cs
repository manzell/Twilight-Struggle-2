using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class AdjustInfluence : PlayerAction
{
    [SerializeField] Dictionary<CountryFaction, int> influencePlacement = new();
    [SerializeField] int influencePool;
    [SerializeField] int maxPerCountry;
    [SerializeField] List<Continent> continents = new();
    [SerializeField] List<CountryData> eligibleCountries = new();

    struct CountryFaction
    {
        public CountryData countryData;
        public Faction faction; 

        public CountryFaction(CountryData c, Faction f)
        {
            countryData = c;
            faction = f; 
        }
    }

    public override async Task Action()
    {
        List<Country> countrySelections = new List<Country>();

        // First, execute any influenceChange that's hard-entered
        foreach (CountryFaction influenceKey in influencePlacement.Keys)
            influenceKey.countryData.country.AdjustInfluence(Player.Faction, influencePlacement[influenceKey]);

        if (influencePool != 0)
        {
            int adjustAmount = influencePool / Mathf.Abs(influencePool);

            // If we don't have a list of Specific Countries, any country from the Continents list if we're adding, or any country with enemy influence, if we're removing
            if (eligibleCountries.Count == 0)
            {
                foreach (Continent continent in continents)
                {
                    if (influencePool > 0)
                        eligibleCountries.AddRange(continent.countries.Select(country => country.Data)); 
                    else if(influencePool < 0)
                        eligibleCountries.AddRange(continent.countries.Where(country => country.Influence((Card.Faction ?? Player.Faction).enemyFaction) > 0).Select(country => country.Data));
                }
            }

            //If we still don't have any countries, let's add countries where we have influence as well as their neighbors
            if(eligibleCountries.Count == 0 && continents.Count == 0)
                foreach(Country country in Game.Countries.Where(c1 => c1.Influence(Player.Faction) > 0 || c1.Neighbors.Any(c2 => c2.country.Influence(Player.Faction) > 0)))
                    if(!eligibleCountries.Contains(country.Data)) 
                        eligibleCountries.Add(country.Data);

            SelectionManager<Country> selectionManager = new(eligibleCountries.Select(countryData => countryData.country), country => {
                if (influencePool != 0 && countrySelections.Count(c => c == country) < maxPerCountry)
                {
                    countrySelections.Add(country);
                    country.AdjustInfluence(adjustAmount > 0 ? Player.Faction : Player.Enemy.Faction, adjustAmount);
                    influencePool -= adjustAmount;
                }
            });                

            while (selectionManager.open && influencePool != 0)
            {
                twilightStruggle.UI.UI_Message.SetMessage($"{(influencePool > 0 ? "Add" : "Remove")} {(influencePool > 0 ? Player : Player.Enemy).name} Influence ({Mathf.Abs(influencePool)} remaining)"); 

                await selectionManager.Selection;

                // Removes any countries where we've reached our max. Move this to the Selection Manager? 
                selectionManager.RemoveSelectables(selectionManager.Selectables
                    .Where(country => countrySelections.Count(country2 => country2 == country) >= maxPerCountry).ToArray());

                // If we're removing influence, remove any country which no longer has enemy influence
                if (influencePool < 0)
                    selectionManager.RemoveSelectables(selectionManager.Selectables
                        .Where(country => country.Influence(Card.Faction.enemyFaction ?? Player.Faction.enemyFaction) == 0).ToArray()); 
            }

            selectionManager.Close(); 
        }
    }
}
