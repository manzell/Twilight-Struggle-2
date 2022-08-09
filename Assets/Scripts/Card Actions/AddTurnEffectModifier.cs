using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AddTurnEffectModifier : PlayerAction
{
    [SerializeField] Modifier _modifier;
    [SerializeField] Effect effect;
    Turn turn; 

    protected override Task Action()
    {
        turn = Phase.GetCurrent<Turn>();

        if (turn != null)
        {
            if(_modifier != null)
            {
                turn.modifiers.Add(_modifier);
                turn.phaseEndEvent += () => turn.modifiers.Remove(_modifier);
            }
            if(effect != null)
            {
                turn.activeEffects.Add(effect);
                turn.phaseEndEvent += () => Game.activeEffects.Remove(effect); 
            }
        }

        return Task.CompletedTask; 
    }
}
