using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class UNEffect : PlayerAction
{
    [SerializeField] Place placeEffect;

    public override async Task Action()
    {
        SelectionManager<Card> selectionManager = new(Player.hand.Where(card => card.Faction == Player.Faction.enemyFaction));

        PlayCard playCard = new PlayCard();
        Card card = await selectionManager.Selection as Card; 
        playCard.SetCard(card);
        playCard.RemoveAction(typeof(TriggerCardEvent));
        selectionManager.Close();

        await playCard.Event(this); 
    }
}
