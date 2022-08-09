using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class PreventHeadline : PlayerAction
{
    public override bool Can(Player player, Card card) => false;

    protected override Task Action()
    {
        return Task.CompletedTask; 
    }
}