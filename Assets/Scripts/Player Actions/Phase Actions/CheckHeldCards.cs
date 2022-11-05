using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    public class CheckHeldCards : PhaseAction
    {
        public override Task Do(Phase phase)
        {
            Player loser = Game.Players.Where(player => player.hand.Any(card => card.Data is ScoringCard))
                .OrderByDescending(player => player.name)
                .First();

            if (loser != null)
            {
                Debug.Log($"{loser.name} held a scoring card and loses the game!");
                Game.EndGame();
            }

            return Task.CompletedTask;
        }
    }
}