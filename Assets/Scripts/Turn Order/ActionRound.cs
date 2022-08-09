using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; 

public class ActionRound : Phase
{
    public Player phasingPlayer;
    public int actionRoundNumber;
    public bool opponentEventTriggered;
    
    public async override Task DoPhase(Phase parent)
    {
        FindObjectOfType<UI_Hand>().SetPlayer(phasingPlayer);
        await base.DoPhase(parent); 
    }
}
