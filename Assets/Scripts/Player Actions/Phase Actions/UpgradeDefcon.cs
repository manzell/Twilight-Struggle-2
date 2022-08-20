using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UpgradeDefcon : PhaseAction
{
    public override Task Do(Phase phase)
    {
        Game.AdjustDEFCON(1);
        return Task.CompletedTask; 
    }
}
