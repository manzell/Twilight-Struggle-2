using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DuckVPEffect : PlayerAction
{
    protected override Task Action()
    {
        Game.AdjustVP(Player.faction == (Card.Faction ?? Player.faction) ? Player : Player.enemyPlayer, 5 - Game.DEFCON);
        
        return Task.CompletedTask; 
    }
}
