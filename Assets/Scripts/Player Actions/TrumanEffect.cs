using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace TwilightStruggle
{
    public class TrumanEffect : PlayerAction
    {
        [SerializeField] Continent europe;

        public override async Task Action()
        {
            IEnumerable<Country> eligibleCountries = Game.Countries.Where(c => c.Continents.Contains(europe) && c.Control == null);

            if (eligibleCountries.Count() > 0)
            {
                TwilightStruggle.UI.Message.SetMessage($"Truman Doctrine in effect. Remove all USSR Influence from one uncontrolled country in Europe");
                SelectionManager<Country> selectionManger = new(eligibleCountries);

                Country country = await selectionManger.Selection as Country;
                country.SetInfluence(Player.Enemy.Faction, 0);

                selectionManger.Close();
            }
        }
    }
}