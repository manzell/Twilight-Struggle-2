using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class UNEffect : PlayerAction
{
    [SerializeField] Place placeEffect; 

    protected override async Task Action(Player player, Card card)
    {
        SelectionManager<Card> selectionManager = new(player.hand.Where(card => card.Faction == player.faction.enemyFaction), PlayCardForOps);
        await selectionManager.Selection;
        selectionManager.Close(); 

        void PlayCardForOps(Card card)
        {

        }
    }
}
