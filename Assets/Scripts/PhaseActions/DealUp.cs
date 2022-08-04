using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class DealUp : PhaseAction
{
    public override async Task Do(Phase phase)
    {
        Debug.Log($"Dealing up to : {Game.currentPhase.GetCurrent<Turn>().warPhase.handSize} cards");

        while (Game.Players.Count(f => f.hand.Count < phase.GetCurrent<Turn>().warPhase.handSize) > 0)
            foreach (Player player in Game.Players.Where(p => p.hand.Count < phase.GetCurrent<Turn>().warPhase.handSize))
                await Game.Draw(player);
    }
}
