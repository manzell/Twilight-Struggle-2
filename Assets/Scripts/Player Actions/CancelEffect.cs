using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CancelEffect : PlayerAction
{
    [SerializeField] Effect effect;
    public override Task Action()
    {
        //  Game.RemoveEffect(effect); 

        return Task.CompletedTask; 
    }
}
