using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/ArmsRaceVP")]
public class AwardArmsRaceVP : PlayerAction
{
    protected override Task Action()
    {
        Debug.Log($"Event({Card.name}, {Player.name}) received");
        if (Player.milOps > Player.Enemy.milOps && Player.milOps >= Game.DEFCON)
            Player.AdjustVP(3);
        else if (Player.milOps > Player.Enemy.milOps)
            Player.AdjustVP(1);

        return Task.CompletedTask; 
    }
}
