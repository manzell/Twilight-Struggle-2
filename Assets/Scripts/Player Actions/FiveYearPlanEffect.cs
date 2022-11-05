using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class FiveYearPlanEffect : PlayerAction
    {
        [SerializeField] Faction USSR;

        public override async Task Action()
        {
            if (USSR.player.hand.Count > 0)
            {
                Card discardedCard = USSR.player.hand.OrderBy(c => Random.value).First();
                Debug.Log($"USSR Discards {discardedCard.name} to Five Year Plan");

                if (discardedCard.Faction == USSR.enemyFaction)
                    await discardedCard.Event(USSR.player);
            }
        }
    }
}