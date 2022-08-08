using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RevealHand : PlayerAction
{
    protected override async Task Action(Player player, Card card)
    {
        Faction factionFaction = card.Faction ?? player.faction;
        List<Card> revealedHand = factionFaction.enemyFaction.player.hand; 

        SelectionManager<Card> selection = new(revealedHand, 0);
        await selection.Selection; 
    }
}
