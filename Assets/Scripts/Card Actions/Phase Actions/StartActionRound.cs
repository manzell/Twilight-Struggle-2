using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//create event listener
// use a callback to redirect to the permanent event listener that chooseAction is representing
public class StartActionRound : PhaseAction
{
    public TaskCompletionSource<PlayerAction> actionChoice { get; private set; }

    public async override Task Do(Phase phase)
    {
        actionChoice = new();
        (phase as ActionRound).SetActionRoundStart(this);

        await actionChoice.Task; 
    }
}
