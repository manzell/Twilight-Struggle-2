using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DuckVPEffect : CardEffect
{
    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        Game.AdjustVP(player.faction == (card.Faction ?? player.faction) ? player : player.enemyPlayer, 5 - Game.DEFCON);
        
        return Task.CompletedTask; 
    }
}
