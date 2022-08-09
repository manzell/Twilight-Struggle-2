using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RevealHand : PlayerAction
{
    protected override async Task Action()
    {
        List<Card> revealedHand = (Card.Faction ?? Player.faction).enemyFaction.player.hand; 

        SelectionManager<Card> selection = new(revealedHand);
        await selection.Selection;
        selection.Close(); 
    }
}
