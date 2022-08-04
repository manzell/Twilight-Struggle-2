using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class CambridgeEffect : CardEffect
{
    [SerializeField] Player USA;
    [SerializeField] Faction USSR;
    [SerializeField] WarPhase lateWar; 
    CardSelectionManager cardSelection;
    CountrySelectionManager countrySelection;

    public override bool Can(Card card, Player player)
    {
        if (Game.currentPhase.GetCurrent<Turn>().warPhase == lateWar) return false; 
        return base.Can(card, player);
    }

    public override async Task Event(Card card, Player player)
    {
        List<Country> eligibleCountries = new();

        foreach (Card c in USA.hand)
            if (c.Data is ScoringCard scoringCard)
                eligibleCountries.AddRange(scoringCard.continent.countries);

        countrySelection = new CountrySelectionManager(eligibleCountries, Place);

        while (countrySelection.selectedCountries.Count == 0)
            await Task.Yield();

        countrySelection.CloseSelectionManager();

        void Place(Country country) => country.AdjustInfluence(USSR, 1);
    }
}
