using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class BlockadeEffect : PlayerAction
{
    [SerializeField] Faction USA;
    [SerializeField] CountryData WestGermany;

    protected override async Task Action(Player player, Card card)
    {
        twilightStruggle.UI.UI_Message.SetMessage("Blockade. US must discard a card with 3 or more Ops, otherwise lose all influence in West Germany"); 
        List<Card> eligibleCards = USA.player.hand.Where(card => card.ops >= 3).ToList();

        if (eligibleCards.Count == 0)
            WestGermany.country.SetInfluence(USA, 0);
        else
        {
            SelectionManager<Card> selection = new (USA.player.hand.Where(card => card.ops >= 3), 1);

            while (selection.open && selection.Selected.Count() == 0)
                await selection.Selection;

            selection.Close(); 

            if (selection.Selected.Count() > 0)
                USA.player.Discard(selection.Selected.First());
            else
                WestGermany.country.SetInfluence(USA, 0); 
        }
    }
}
