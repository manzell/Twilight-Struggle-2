using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PreventEffect : PlayerAction
{
    [SerializeField] Effect effect; 

    protected override Task Action()
    {
        return Task.CompletedTask; 
    }
}
