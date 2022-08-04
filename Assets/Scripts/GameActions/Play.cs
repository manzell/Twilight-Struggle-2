using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

[CreateAssetMenu]
public class Play : GameAction
{
    public static event Action<Play> prepPlay, playEvent, endPlay;

    public override async Task Event(Player player, Card card)
    {
        this.card = card; 

        prepPlay?.Invoke(this); 
        await card.Event(player);
        playEvent?.Invoke(this); 
        endPlay?.Invoke(this); 

        // If the player is playing a friendly card for the event, then move to the next phase. 
        // Otherwise, we want to make that the CardPlayEvent of an enemy card happened, but not advance the round. 
        if(card.Faction != player.enemyPlayer.faction)
            Game.currentPhase.Continue(); 
    }
}
