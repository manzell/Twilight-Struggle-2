using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEditor.EditorTools;

namespace TwilightStruggle
{
    public class AddBonusInfluence : PhaseAction
    {
        [SerializeField] Faction faction;
        [SerializeField] int influence;

        public async override Task Do(Phase phase)
        {
            IEnumerable<Country> selectableCountries = Game.Countries.Where(country => country.Influence(faction) > 0);

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