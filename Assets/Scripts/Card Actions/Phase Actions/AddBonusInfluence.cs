using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class AddBonusInfluence : PhaseAction
{
    [SerializeField] Faction faction;
    [SerializeField] int influence;

    public async override Task Do(Phase phase)
    {
        IEnumerable<Country> countries = Game.Countries.Where(country => country.Influence(faction) > 0);

        if(countries.Count() > 0)
        {
            SelectionManager<Country> selectionManager = new (countries, country => {
                country.AdjustInfluence(faction, 1);
                influence--;
            });

            while (selectionManager.open && influence > 0)
            {
                twilightStruggle.UI.UI_Message.SetMessage($"Add Bonus {faction} Influence ({influence} remaining)");
                await selectionManager.Selection;
            }

            selectionManager.Close(); 
        } 
    }
}
