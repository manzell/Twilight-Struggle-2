using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    public class AwardMilOps : PhaseAction
    {
        public override Task Do(Phase phase)
        {
            Turn turn = phase.Get<Turn>();
            KeyValuePair<Player, int> player1 = turn.milOps.First();
            KeyValuePair<Player, int> player2 = turn.milOps.Last();

            player1.Key.AdjustVP(-Mathf.Clamp(Game.DEFCON - player1.Value, 0, 5)
                - Mathf.Clamp(Game.DEFCON - player2.Value, 0, 5));

            return Task.CompletedTask;
        }
    }
}