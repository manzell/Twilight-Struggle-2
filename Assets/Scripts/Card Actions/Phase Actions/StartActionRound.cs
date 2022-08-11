using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StartActionRound : PhaseAction
{
    public async override Task Do(Phase phase)
    {
        (phase as ActionRound).SetActionRoundStart(this);

        twilightStruggle.UI.UI_Message.SetMessage($"Play {(phase as ActionRound).phasingPlayer.name} Action Round");
        Game.NewActionChoice(); 
        await Game.ActionChoice; // <- this is called only once an Action Gets Triggered from a Card Drag Drop. 
        Game.ResetActionChoice(null); 
    }
}
