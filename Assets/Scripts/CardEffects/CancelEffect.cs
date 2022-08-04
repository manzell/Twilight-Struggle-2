using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Cancel Effect")]
public class CancelEffect : CardEffect
{
    //[SerializeField] OngoingEffect effect; 
    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        //  Game.RemoveEffect(effect); 

        return Task.CompletedTask; 
    }
}
