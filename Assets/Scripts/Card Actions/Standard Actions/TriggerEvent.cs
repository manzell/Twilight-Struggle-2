using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; 
using System.Threading.Tasks;

public class TriggerEvent : PlayerAction
{
    [SerializeField] PlayCard triggerAfterEvent; 
    public static event Action<TriggerEvent> prepPlay, playEvent, endPlay;

    protected override async Task Action()
    {
        ActionRound ar = Phase.GetCurrent<ActionRound>();

        prepPlay?.Invoke(this); 

        await Card.Event(Card.Faction.player ?? Player);

        playEvent?.Invoke(this); 
        endPlay?.Invoke(this);

        // We need to determine if we've already used any of the actions available on the upcoming Choose Event already in our stack. 

        // The below returns TRUE when we've already completed any of the Actions that we've already done this turn. This may be a problem for Star Wars in the Late War as it might fail
        // under recursion. The purpose is to determine when a card is PLAYED, if we need to either present the player the normal usage after Eventing the Card, or Event the card after
        // normal usage. This is the pervue of the ChooseCardAction to keep track of. 
        //if (!Phase.GetCurrent<Phase>().cardActions.Any(previousAction => triggerAfterEvent.Actions.Any(upcomingAction => upcomingAction.GetType() == previousAction.GetType())))

        if(triggerAfterEvent != null)
            await triggerAfterEvent.Event(this); 
    }
}
