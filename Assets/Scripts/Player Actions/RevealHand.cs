using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RevealHand : PlayerAction
{
    public override async Task Action()
    {
        List<Card> revealedHand = (Card.Faction ?? Player.Faction).enemyFaction.player.hand; 

        SelectionManager<Card> selectionManager = new(revealedHand);
        await selectionManager.Selection;
        selectionManager.Close(); 
    }
}
