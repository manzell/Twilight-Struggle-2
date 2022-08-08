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

    protected override async Task Action(Player player, Card card)
    {
        List<Country> clickCounter = new List<Country>();

        // First, execute any influenceChange that's hard-entered
        foreach (CountryFaction influenceKey in influencePlacement.Keys)
            influenceKey.countryData.country.AdjustInfluence(player.faction, influencePlacement[influenceKey]);

        if (influencePool != 0)
        {
            int adjustAmount = influencePool / Mathf.Abs(influencePool);

            // If we don't have a list of Specific Countries, any country from the Continents list
            if (eligibleCountries.Count == 0)
                foreach (Continent continent in continents)
                    eligibleCountries.AddRange(continent.countries.Where(c => influencePool > 0 || 
                        influencePool < 0 && c.Influence(card.Faction.enemyFaction ?? player.faction.enemyFaction) > 0).Select(c => c.Data)); 

            //If we still don't have any countries, let's add countries where we have influence as well as their neighbors
            if(eligibleCountries.Count == 0)
                foreach(Country country in Game.Countries.Where(c1 => c1.Influence(player.faction) > 0 || c1.Neighbors.Any(c2 => c2.country.Influence(player.faction) > 0)))
                    eligibleCountries.Add(country.Data);

            SelectionManager<Country> selectionManager = new(eligibleCountries.Select(c => c.country), s =>
            {
               Country country = s as Country;
               if (influencePool != 0 && clickCounter.Count(c => c == country) < maxPerCountry)
               {
                   country.AdjustInfluence(adjustAmount > 0 ? player.faction : player.enemyPlayer.faction, adjustAmount);
                   influencePool -= adjustAmount;
                   clickCounter.Add(country);
               }
            });                

            while (selectionManager.open && influencePool != 0)
            {
                await selectionManager.Selection;

                foreach (Country c in selectionManager.Selectable.Where(c1 => clickCounter.Count(c2 => c2 == c1) >= maxPerCountry ||
                    influencePool < 0 && c1.Influence(card.Faction.enemyFaction ?? player.faction.enemyFaction) == 0).ToList())
                        selectionManager.RemoveSelectable(c);                 
            }

            selectionManager.Close(); 
        }
    }
}
