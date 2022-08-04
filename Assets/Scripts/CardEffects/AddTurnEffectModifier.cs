using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AddTurnEffectModifier : CardEffect
{
    [SerializeField] Modifier modifier;
    [SerializeField] Effect effect;
    Turn turn; 

    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        turn = Game.currentPhase.GetCurrent<Turn>();

        if (turn != null)
        {
            if(modifier != null)
            {
                turn.modifiers.Add(modifier);
                turn.phaseEndEvent += () => turn.modifiers.Remove(modifier);
            }
            if(effect != null)
            {
                Game.activeEffects.Add(effect);
                turn.phaseEndEvent += () => Game.activeEffects.Remove(effect); 
            }
        }

        return Task.CompletedTask; 
    }
}
