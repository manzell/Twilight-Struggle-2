using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System; 

public class UI_CardplayDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField] GameAction original;
    [SerializeField] UI_Hand handObject;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.selectedObject.TryGetComponent(out UI_Card c))
        {
            Player player = handObject.currentPlayer;
            Card card = c.card;

            if (original.Can(player, card))
            {
                GameAction dropAction = Instantiate(original); 
                dropAction.Event(player, card);
                Game.currentPhase.gameActions.Add(dropAction);
                Game.InvokePlayEvent(player, card);
            }            
        }
    }
}
