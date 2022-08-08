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
    [SerializeField] UI_Hand handObject;
    [SerializeField] UI_ActionSelection actionSelection;
    [SerializeField] TextMeshProUGUI actionName;

    public void Setup(UI_ActionSelection actionSelection, PlayerAction p)
    {
        Debug.Log($"Action Selection: {p}");

        this.actionSelection = actionSelection;
        actionName.text = p.GetType().ToString(); 
        dropAction = p;
    } 

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.selectedObject.TryGetComponent(out UI_Card c))
        {
            var action = Activator.CreateInstance(dropAction.GetType()) as PlayerAction;
            action.SetCard(c.card); 
            actionSelection.Select(action); 
        }
    }
}
