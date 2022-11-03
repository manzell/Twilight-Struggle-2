using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace twilightStruggle
{
    public class ActionRound : Phase
    {
        public Player phasingPlayer;
        public int actionRoundNumber;
        public bool opponentEventTriggered;

        public override void StartPhase(Phase parent)
        {
            UI_PlayerBoard.SetPlayer(phasingPlayer);
            base.StartPhase(parent); 
        }

        public override void OnPhase()
        {
            UI.UI_Message.SetMessage($"Play {phasingPlayer.name} Action Round (Turn {GetComponentInParent<Turn>().turnNumber} AR {actionRoundNumber})");
        }
    }
}