using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class TriggerEvent : PlayerAction
{
    public static event Action<TriggerEvent> prepPlay, playEvent, endPlay;

    protected override async Task Action(Player player, Card card)
    {
        ActionRound ar = Phase.GetCurrent<ActionRound>();

        prepPlay?.Invoke(this); 

        await card.Event(player);

        playEvent?.Invoke(this); 
        endPlay?.Invoke(this);

        if (card.Faction == player.faction.enemyFaction && ar != null)
            ar.opponentEventTriggered = true;
    }
}
