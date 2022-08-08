using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class DestalEffect : PlayerAction
{
    [SerializeField] int maxRelocate, maxPlacePerCountry; 
    [SerializeField] Faction USA; 
 
    protected override async Task Action(Player player, Card card)
    {
        twilightStruggle.UI.UI_Message.SetMessage($"De-Stalinization: Select up to {maxRelocate} influence to relocate");
        int influenceRemoved = 0; 
        List<Country> eligibleCountries = Game.Countries.Where(c => c.Influence(card.Faction) > 0).ToList();
        List<Country> placedCountries = new();

        SelectionManager<Country> selection = new(eligibleCountries, country => {
            country.AdjustInfluence(card.Faction, -1);
            influenceRemoved++;
        });

        while (selection.Selected.Count() < maxRelocate && selection.open)
        {
            await selection.Selection;
            foreach (Country country in selection.Selectable.Where(c => c.Influence(card.Faction) == 0))
                selection.RemoveSelectable(country);
        }

        if(influenceRemoved > 0)
        {
            selection = new(Game.Countries.Where(country => country.Control != USA), country => {
                country.AdjustInfluence(card.Faction, 1);
                influenceRemoved--;
            });

            while (selection.open || influenceRemoved > 0)
            {
                twilightStruggle.UI.UI_Message.SetMessage($"De-Stalinization: Place {influenceRemoved} more influence in any non-US controlled country");
                foreach (Country country in selection.Selected.Where(c => selection.Selected.Count(c2 => c2 == c) >= maxPlacePerCountry))
                    selection.RemoveSelectable(country);

                await selection.Selection; 
            }
        }
    }
}