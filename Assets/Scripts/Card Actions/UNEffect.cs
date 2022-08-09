using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class UNEffect : PlayerAction
{
    [SerializeField] Place placeEffect; 

    protected override async Task Action()
    {
        SelectionManager<Card> selectionManager = new(Player.hand.Where(card => card.Faction == Player.faction.enemyFaction));

        PlayCard playCard = new PlayCard();
        playCard.SetCard(await selectionManager.Selection);
        playCard.RemoveAction(typeof(TriggerEvent));
        selectionManager.Close();

        await playCard.Event(this); 
    }
}
