using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class FiveYearPlanEffect : PlayerAction
{
    [SerializeField] Faction USSR;

    protected override async Task Action(Player player, Card card)
    {
        if(USSR.player.hand.Count > 0)
        {
            Card discard = USSR.player.hand.OrderBy(c => Random.value).First();
            Debug.Log($"USSR Discards {discard.name} to {card.name}");

            if (discard.Faction == USSR.enemyFaction)
                await card.Event(USSR.player);
        }
    }
}
