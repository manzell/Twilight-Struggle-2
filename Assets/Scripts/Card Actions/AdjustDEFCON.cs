using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AdjustDEFCON : PlayerAction
{
    [SerializeField] int DEFCONAdjustment;

    protected override Task Action()
    {
        Game.AdjustDEFCON(DEFCONAdjustment);
        return Task.CompletedTask; 
    }
}
