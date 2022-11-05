using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class DealUp : PhaseAction
    {
        public override async Task Do(Phase phase)
        {
            int handsize = Phase.GetCurrent<Turn>().warPhase.handSize;
            Debug.Log($"Dealing up to {handsize} cards");

            while (Game.Players.Count(f => f.hand.Count < handsize) > 0)
                foreach (Player player in Game.Players.Where(p => p.hand.Count < handsize))
                    await Game.Draw(player);
        }
    }
}