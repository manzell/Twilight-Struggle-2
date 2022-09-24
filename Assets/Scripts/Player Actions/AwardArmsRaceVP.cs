using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AwardArmsRaceVP : PlayerAction
{
    public override Task Action()
    {
        if (Player.milOps > Player.Enemy.milOps && Player.milOps >= Game.DEFCON)
            Player.AdjustVP(3);
        else if (Player.milOps > Player.Enemy.milOps)
            Player.AdjustVP(1);

        return Task.CompletedTask; 
    }
}
