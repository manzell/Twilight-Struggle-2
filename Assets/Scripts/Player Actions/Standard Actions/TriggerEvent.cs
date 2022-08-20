using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; 
using System.Threading.Tasks;

public class TriggerEvent : PlayerAction
{
    [SerializeField] PlayCard triggerAfterEvent; 
    public static event Action<TriggerEvent> prepEvent, playEvent;

    public override async Task Action()
    {
        prepEvent?.Invoke(this); 

        await Card.Event(Card.Faction?.player ?? Player);

        playEvent?.Invoke(this); 

        if(triggerAfterEvent != null)
            await triggerAfterEvent.Event(this); 
    }
}
