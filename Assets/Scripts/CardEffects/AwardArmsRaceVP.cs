using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/ArmsRaceVP")]
public class AwardArmsRaceVP : CardEffect
{
    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        if (player.milOps > player.enemyPlayer.milOps && player.milOps >= Game.DEFCON)
            player.AdjustVP(3);
        else if (player.milOps > player.enemyPlayer.milOps)
            player.AdjustVP(1);

        return Task.CompletedTask; 
    }
}
