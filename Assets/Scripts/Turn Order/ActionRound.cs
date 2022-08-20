using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; 

public class ActionRound : Phase
{
    public Player phasingPlayer;
    public int actionRoundNumber;
    public bool opponentEventTriggered;
    public StartActionRound startActionRound { get; private set; }
    public void SetActionRoundStart(StartActionRound startActionRound) => this.startActionRound = startActionRound;

    public async override Task DoPhase(Phase parent)
    {
        UI_PlayerBoard.SetPlayer(phasingPlayer);
        await base.DoPhase(parent); 
    }

}
