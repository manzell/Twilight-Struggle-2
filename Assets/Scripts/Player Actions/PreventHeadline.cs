using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class PreventHeadline : PlayerAction
{
    public override bool Can(Player player, Card card) => false;

    public override Task Action()
    {
        return Task.CompletedTask; 
    }
}