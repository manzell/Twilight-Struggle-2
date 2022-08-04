using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class ActionRound : Phase
{
    public Player phasingPlayer;
    public int actionRoundNumber;
    bool opponentEventTriggered, used;

    public void Play(GameAction action)
    {
        if (!opponentEventTriggered && action.card.Faction == action.player.faction.enemyFaction && (action is Coup || action is Place || action is Realign))
        {
            used = true;
            Play(Instantiate(action)); // TODO this is bad and unreliable!
        }
        else if (action is Space)
            Game.currentPhase.Continue(); 
        else if (action is Play) // This could be friendly-for-event, enemy-event-first, or enemy-after-ops
        {
            if (used || action.card.Faction != action.player.faction.enemyFaction)
                Game.currentPhase.Continue();
            else
                opponentEventTriggered = true; 
        }

        if(opponentEventTriggered || action is Play)
            (action.card.Data.removeOnEvent ? Game.removed : Game.discards).Add(action.card); 
    }
}
