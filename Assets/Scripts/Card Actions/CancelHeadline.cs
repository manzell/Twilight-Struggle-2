using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CancelHeadline : PlayerAction
{
    protected override Task Action(Player player, Card card) => Task.CompletedTask; 
}
