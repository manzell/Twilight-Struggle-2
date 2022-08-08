using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Cancel Effect")]
public class CancelEffect : PlayerAction
{
    //[SerializeField] OngoingEffect effect; 
    protected override Task Action(Player player, Card card)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        //  Game.RemoveEffect(effect); 

        return Task.CompletedTask; 
    }
}
