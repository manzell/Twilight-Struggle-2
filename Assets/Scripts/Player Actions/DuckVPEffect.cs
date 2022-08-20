using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DuckVPEffect : PlayerAction
{
    public override Task Action()
    {
        (Player.Faction == (Card.Faction ?? Player.Faction) ? Player : Player.Enemy).AdjustVP(5 - Game.DEFCON);         
        return Task.CompletedTask; 
    }
}
