using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using System.Linq;
using System.Threading.Tasks;

public class AddStartingInfluence : PhaseAction 
{
    [SerializeField] Continent continent;
    [SerializeField] Faction faction;
    [SerializeField] int influence;

    public override async Task Do(Phase phase)
    {
        IEnumerable<Country> selectableCountries = Game.Countries.Where(country => country.Continents.Contains(continent)); 

        if (selectableCountries.Count() > 0)
        {
            SelectionManager<Country> selectionManager = new (selectableCountries);

            while (selectionManager.open && selectionManager.Selected.Count() < influence)
            {
                twilightStruggle.UI.UI_Message.SetMessage($"Add Starting {faction} Influence ({influence} remaining)");                
                (await selectionManager.Selection).AdjustInfluence(faction, 1);
            }

            selectionManager.Close(); 
        }
    }
}