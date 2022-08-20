using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using TMPro;
using System.Linq;

public class UI_PlayerAction : SerializedMonoBehaviour, IDropHandler
{
    public PlayerAction Action => action;
    public static event Action<PlayerAction, Card> cardDropEvent; 
    [SerializeField] PlayerAction action;
    [SerializeField] Image backgroundImage; 
    [SerializeField] UI_SelectionManager uiSelectionManager;
    [SerializeField] TextMeshProUGUI actionName, submittedCardName;

    public void Setup(UI_SelectionManager uiSelectionManager, PlayerAction action)
    {
        this.uiSelectionManager = uiSelectionManager;
        this.action = action;

        actionName.text = action.name;

        if (action is Headline) // Kinda a hack :P
            backgroundImage.color = action.Player.Faction.controlColor; 
    } 

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.selectedObject.TryGetComponent(out UI_Card uiCard)) // Need a way to ensure the proper player is playing. Defered for now. 
        {
            cardDropEvent?.Invoke(action, uiCard.card);
            action.Select(uiCard.card);
            uiSelectionManager.UISelect(action);
        }
    }
}
