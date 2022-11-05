using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class ActionRound : Phase
    {
        public Player phasingPlayer;
        public int actionRoundNumber;
        public bool opponentEventTriggered;

        public override void StartPhase(Phase parent)
        {
            foreach (PlayerAction playerAction in availableActions)
                playerAction.SetPlayer(phasingPlayer);

            UI.PlayerBoard.SetPlayer(phasingPlayer);
            base.StartPhase(parent); 
        }

        public override void OnPhase()
        {
            UI.Message.SetMessage($"Play {phasingPlayer.name} Action Round (Turn {GetComponentInParent<Turn>().turnNumber} AR {actionRoundNumber})");
        }
    }
}