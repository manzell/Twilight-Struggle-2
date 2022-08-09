using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StartActionRound : PhaseAction
{
    [SerializeField] PlayCard chooseAction;
    public async override Task Do(Phase phase)
    {
        if (phase is ActionRound actionRound)
            await chooseAction.Event(actionRound.phasingPlayer); 
    }
}
