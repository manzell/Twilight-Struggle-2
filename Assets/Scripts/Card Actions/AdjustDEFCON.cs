using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AdjustDEFCON : PlayerAction
{
    [SerializeField] int DEFCONAdjustment;

    protected override Task Action(Player player, Card card)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        Player povPlayer = player.faction == (card.Faction ?? player.faction) ? player : player.enemyPlayer; 

        Game.AdjustDEFCON(DEFCONAdjustment);

        return Task.CompletedTask; 
    }
}
