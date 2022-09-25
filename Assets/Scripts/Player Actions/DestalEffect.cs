using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;

public class DestalEffect : PlayerAction
{
    [SerializeField] int maxRelocate, maxPlacePerCountry; 
    [SerializeField] Faction USA; 
 
    public override async Task Action()
    {
        twilightStruggle.UI.UI_Message.SetMessage($"De-Stalinization: Select up to {maxRelocate} influence to relocate");
        int influenceRemoved = 0; 
        List<Country> eligibleCountries = Game.Countries.Where(c => c.Influence(USA.enemyFaction) > 0).ToList();
        List<Country> placedCountries = new();

        SelectionManager<Country> selection = new(eligibleCountries, country => {
            (country as Country).AdjustInfluence(USA.enemyFaction, -1);
            influenceRemoved++;
        });

        while (selection.Selected.Count() < maxRelocate && selection.open)
        {
            await selection.Selection;

            List<ISelectable> countries = selection.Selectables.Where(c => (c as Country).Influence(USA) == 0).ToList(); 

            foreach (Country country in countries)
                selection.RemoveSelectable(country);
        }

        if(influenceRemoved > 0)
        {
            selection = new(Game.Countries.Where(country => country.Control != USA), country => {
                (country as Country).AdjustInfluence(USA.enemyFaction, 1);
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
