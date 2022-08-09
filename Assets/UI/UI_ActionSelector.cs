using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using TMPro;

public class UI_ActionSelector : SerializedMonoBehaviour, IDropHandler
{
    [SerializeField] PlayerAction dropAction; 
    [SerializeField] UI_ActionSelection actionSelection;
    [SerializeField] TextMeshProUGUI actionName;
    Player requiredPlayer; 

    public PlayerAction Action => dropAction; 

    public void Setup(UI_ActionSelection actionSelection, Player player, PlayerAction p)
    {
        this.actionSelection = actionSelection;
        this.requiredPlayer = player; 
        actionName.text = p.GetType().ToString(); 
        dropAction = p;
    } 

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.selectedObject.TryGetComponent(out UI_Card uiCard))
        {
            if(requiredPlayer == null || requiredPlayer.hand.Contains(uiCard.card))
            {
                PlayerAction action = dropAction.Clone(); 

                // Warning: Shitty hack here. Instead we want to move 
                //var action = Activator.CreateInstance(dropAction.GetType()) as PlayerAction;

                //if (action is Space space)
                //    space.SetSpaceRace((dropAction as Space).GetSpaceRace()); 

                // Controversial: Remove the card from the player's hand here. 
                requiredPlayer?.hand.Remove(uiCard.card); 

                action.SetCard(uiCard.card);
                actionSelection.Select(action);
            }
        }
    }
}
