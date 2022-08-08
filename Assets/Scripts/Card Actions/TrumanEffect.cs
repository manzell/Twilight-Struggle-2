using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class TrumanEffect : PlayerAction
{
    [SerializeField] Continent europe;

    protected override async Task Action(Player player, Card card)
    {
        IEnumerable<Country> eligibleCountries = Game.Countries.Where(c => c.Continents.Contains(europe) && c.Control == null); 

        if(eligibleCountries.Count() > 0)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Truman Doctrine in effect. Remove all USSR Influence from one uncontrolled country in Europe");
            SelectionManager<Country> selectionManger = new(eligibleCountries, country => country.SetInfluence(player.enemyPlayer.faction, 0));
            await selectionManger.Selection;
            selectionManger.Close();
        }
    }
}
