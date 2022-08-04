using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName ="CardEffect/Clear or Control Country")]
public class ClearOrControlCountry : CardEffect
{
    enum CountryAction { Clear, Control, Both }

    [SerializeField] Country country;
    [SerializeField] CountryAction action; 

    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        Faction faction = card.Faction ?? player.faction; 

        if(action == CountryAction.Control)
            country.SetInfluence(faction, country.Influence(faction.enemyFaction) + country.Stability);
        else if (action == CountryAction.Clear)
            country.SetInfluence(faction.enemyFaction, 0);

        return Task.CompletedTask; 
    }
}
