using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CancelHeadline : PlayerAction
{
    public override Task Action() => Task.CompletedTask; 
}