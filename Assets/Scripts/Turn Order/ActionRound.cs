using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; 

public class ActionRound : Phase
{
    public Player phasingPlayer;
    public int actionRoundNumber;
    public bool opponentEventTriggered;

    //public void Play(CardAction action)
    //{
    //    if (!opponentEventTriggered && action.card.Faction == action.player.faction.enemyFaction && (action is Coup || action is Place || action is Realign))
    //    {
    //        used = true;
    //        Play(Instantiate(action)); // TODO this is bad and unreliable!
    //    }
    //    else if (action is Space)
    //        Game.currentPhase.Continue(); 
    //    else if (action is TriggerEvent) // This could be friendly-for-event, enemy-event-first, or enemy-after-ops
    //    {
    //        if (used || action.card.Faction != action.player.faction.enemyFaction)
    //            Game.currentPhase.Continue();
    //        else
    //            opponentEventTriggered = true; 
    //    }

    //    if(opponentEventTriggered || action is TriggerEvent)
    //        (action.card.Data.removeOnEvent ? Game.removed : Game.discards).Add(action.card); 
    //}
}
