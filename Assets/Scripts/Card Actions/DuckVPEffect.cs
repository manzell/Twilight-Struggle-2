using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DuckVPEffect : PlayerAction
{
    protected override Task Action(Player player, Card card)
    {
        Game.AdjustVP(player.faction == (card.Faction ?? player.faction) ? player : player.enemyPlayer, 5 - Game.DEFCON);
        
        return Task.CompletedTask; 
    }
}
