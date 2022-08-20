using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class BlockadeEffect : PlayerAction
{
    [SerializeField] Faction USA;
    [SerializeField] CountryData WestGermany;

    public override async Task Action()
    {
        twilightStruggle.UI.UI_Message.SetMessage("Blockade. US must discard a card with 3 or more Ops, otherwise lose all influence in West Germany"); 
        List<Card> eligibleCards = USA.player.hand.Where(card => card.ops >= 3).ToList();

        if (eligibleCards.Count == 0)
            WestGermany.country.SetInfluence(USA, 0);
        else
        {
            SelectionManager<Card> selectionManager = new (USA.player.hand.Where(card => card.ops >= 3), 1);

            while (selectionManager.open && selectionManager.Selected.Count() == 0)
                await selectionManager.Selection;

            selectionManager.Close(); 

            if (selectionManager.Selected.Count() > 0)
                USA.player.Discard(selectionManager.Selected.First());
            else
                WestGermany.country.SetInfluence(USA, 0); 
        }
    }
}
