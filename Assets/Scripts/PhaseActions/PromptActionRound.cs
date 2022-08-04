using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PromptActionRound : PhaseAction
{
    public override async Task Do(Phase phase)
    {
        // Simply do nothing; 
        /* The AI system will display the proper AI options to play a card
         * 
         * Eventually we should do a while the ActionRoundAction or whatever is null just await Task.Yield()
         * But not yet, need to clarify what the Data Structure is for Action Round Plays
         */
        if(phase is ActionRound ar)
        {
            while (ar.gameActions.Count == 0)
                await Task.Yield();
        }
    }
}
