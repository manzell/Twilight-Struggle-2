using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DuckVPEffect : PlayerAction
{
    [SerializeField] Player USA; 

    public override Task Action()
    {
        USA.AdjustVP(5 - Game.DEFCON);
        return Task.CompletedTask; 
    }
}
