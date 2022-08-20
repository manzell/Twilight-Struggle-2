using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class FiveYearPlanEffect : PlayerAction
{
    [SerializeField] Faction USSR;

    public override async Task Action()
    {
        if(USSR.player.hand.Count > 0)
        {
            Card discard = USSR.player.hand.OrderBy(c => Random.value).First();
            Debug.Log($"USSR Discards {discard.name} to {Card.name}");

            if (discard.Faction == USSR.enemyFaction)
                await Card.Event(Card.Faction.player ?? USSR.player);
        }
    }
}
