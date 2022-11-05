using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using System.Linq;
using System.Threading.Tasks;

namespace TwilightStruggle
{
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
                UI.PlayerBoard.SetPlayer(faction.player);
                SelectionManager<Country> selectionManager = new(selectableCountries);

                while (selectionManager.open && selectionManager.Selected.Count() < influence)
                {
                    UI.Message.SetMessage($"Add Starting {faction} Influence ({influence - selectionManager.Selected.Count()} remaining)");
                    selectionManager.selectionTaskSource = new();
                    Country country = await selectionManager.selectionTaskSource.Task as Country;
                    country.AdjustInfluence(faction, 1);
                }

                selectionManager.Close();
            }
        }
    }
}