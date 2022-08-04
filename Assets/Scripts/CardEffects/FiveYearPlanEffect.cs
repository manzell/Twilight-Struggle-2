using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class FiveYearPlanEffect : CardEffect
{
    Player USSR;
    [SerializeField] Play playEffect; 

    public override async Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        Card c = USSR.hand.OrderBy(c => Random.value).First();

        if(c.Faction == USSR.enemyPlayer.faction)
            await ScriptableObject.Instantiate(playEffect).Event(player, c); 
        else
            USSR.Discard(c);
    }
}
