using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class ClearOrControlCountry : PlayerAction
{
    enum CountryAction { Clear, Control, Both }

    [SerializeField] CountryData countryData;
    [SerializeField] CountryAction action;

    protected override Task Action()
    {
        Faction faction = Card.Faction ?? Player.faction;
        Country country = countryData.country; 

        if(action == CountryAction.Control || action == CountryAction.Both)
            country.SetInfluence(faction, country.Influence(faction.enemyFaction) + country.Stability);
        else if (action == CountryAction.Clear || action == CountryAction.Both)
            country.SetInfluence(faction.enemyFaction, 0);

        return Task.CompletedTask; 
    }
}
