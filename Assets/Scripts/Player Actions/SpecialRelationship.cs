using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class SpecialRelationship : PlayerAction
{
    [SerializeField] CountryData UK;
    [SerializeField] Faction US; 
    [SerializeField] Effect NATO;
    [SerializeField] Continent WesternEurope;

    public override async Task Action()
    {
        if (UK.country.Control == US)
        {
            if(Game.activeEffects.Contains(NATO))
            {
                US.player.AdjustVP(2);
                twilightStruggle.UI.UI_Message.SetMessage("Special Relationship. Add 2 US influence to any country in Western Europe");
                SelectionManager<Country> selectionManager = new(WesternEurope.countries);
                Country country = await selectionManager.Selection as Country;
                country.AdjustInfluence(US, 2);

                selectionManager.Close(); 
            }
            else
            {
                twilightStruggle.UI.UI_Message.SetMessage("Special Relationship. Add 1 US influence to a country neighboring the UK");
                SelectionManager<Country> selectionManager = new(UK.neighbors.Select(n => n.country));

                Country country = await selectionManager.Selection as Country;
                country.AdjustInfluence(US, 1);

                selectionManager.Close(); 
            }
        }
    }
}
