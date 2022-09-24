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

    public override Task Action()
    {
        Country country = countryData.country; 

        if(action == CountryAction.Control || action == CountryAction.Both)
            country.SetInfluence(Player.Faction, country.Influence(Player.Faction.enemyFaction) + country.Stability);
        else if (action == CountryAction.Clear || action == CountryAction.Both)
            country.SetInfluence(Player.Faction.enemyFaction, 0);

        return Task.CompletedTask; 
    }
}
