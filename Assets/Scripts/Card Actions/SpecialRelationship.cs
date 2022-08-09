using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class SpecialRelationship : PlayerAction
{
    [SerializeField] CountryData UK;
    [SerializeField] Faction US; 
    [SerializeField] Effect NATO;
    [SerializeField] Continent WesternEurope; 

    protected override async Task Action()
    {
        if (UK.country.Control == US)
        {
            if(Game.activeEffects.Contains(NATO))
            {
                US.player.AdjustVP(2);
                twilightStruggle.UI.UI_Message.SetMessage("Special Relationship. Add 2 US influence to any country in Western Europe");
                SelectionManager<Country> selectionManager = new(WesternEurope.countries, country => country.AdjustInfluence(US, 2));
                await selectionManager.Selection;
                selectionManager.Close(); 
            }
            else
            {
                twilightStruggle.UI.UI_Message.SetMessage("Special Relationship. Add 1 US influence to a country neighboring the UK");
                SelectionManager<Country> selectionManager = new(UK.neighbors.Select(n => n.country), country => country.AdjustInfluence(US, 1));
                await selectionManager.Selection;
                selectionManager.Close(); 
            }
        }
    }
}
