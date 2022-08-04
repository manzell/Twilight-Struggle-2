using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class BlockadeEffect : CardEffect
{
    [SerializeField] Player USA;
    [SerializeField] Country WestGermany; 

    public override async Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received");
        List<Card> eligibleCards = USA.hand.Where(card => card.ops >= 3).ToList();

        if (eligibleCards.Count == 0)
            Discard(null);
        else
        {
            CardSelectionManager selection = new CardSelectionManager(USA.hand, Discard);

            while (selection.cardSelectionComplete == false)
                await Task.Yield();
            
            Discard(selection.selectedCards.First()); 
        }

        void Discard(Card card)
        {
            if (card != null)
                USA.Discard(card);
            else
                WestGermany.SetInfluence(USA.faction, 0);
        }
    }
}
