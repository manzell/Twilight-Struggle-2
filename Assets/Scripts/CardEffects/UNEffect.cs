using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class UNEffect : CardEffect
{
    [SerializeField] Place placeEffect; 

    public override async Task Event(Card card, Player player)
    {
        Debug.Log($"Event for {card.name} received");

        CardSelectionManager selection = new(player.hand.Where(card => card.Faction == player.faction.enemyFaction), PlayCardForOps);

        while (selection.selectedCards.Count < 1)
            await Task.Yield(); 

        void PlayCardForOps(Card card)
        {
            if(card != null)
            {
                //Place place = ScriptableObject.Instantiate(placeEffect); 
                //await place.Event(card.ops, player);
                //player.Discard(card);
            }
        }
    }
}
