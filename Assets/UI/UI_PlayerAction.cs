using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using TMPro;
using System.Linq;

namespace TwilightStruggle.UI
{
    public class UI_PlayerAction : SerializedMonoBehaviour, IDropHandler
    {
        public PlayerAction Action => action;

        [SerializeField] PlayerAction action;
        [SerializeField] Image backgroundImage;
        [SerializeField] TextMeshProUGUI actionName;
        [SerializeField] GameObject currentActionArea;

        public void Setup(PlayerAction action)
        {
            this.action = action;
            actionName.text = action.name;

            if (action is Headline) // Kinda a hack :P
                backgroundImage.color = action.Player.Faction.controlColor;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.selectedObject.TryGetComponent(out UI_Card uiCard))
            {
                Debug.Log($"Received Drop of {uiCard.card.name}");
                uiCard.transform.localPosition = Vector3.zero; 
                uiCard.transform.SetParent(transform);  // Make the Card a Child of the Action
                transform.SetParent(currentActionArea.transform); // Make the Action a Child of the Current Action instead of the Action Selection Panel
                transform.position = transform.parent.position; 
            }
        }
    }
}
