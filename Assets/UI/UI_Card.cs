using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Linq; 
using Sirenix.OdinInspector; 

public class UI_Card : SerializedMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IHighlightable, IPointerClickHandler
{
    public static event Action<Player, Card> cardDragEvent;
    public static event Action cardEndDragEvent; 

    public Card card;
    [SerializeField] TextMeshProUGUI cardTitle, cardText, cardOps;
    [SerializeField] Image backgroundImage, cardImage, factionIcon;
    [SerializeField] GameObject highlight;

    public void OnPointerClick(PointerEventData eventData) => onClickHandler?.Invoke(card);
    public event Action<Card> onClickHandler;

    public void Setup(Card card)
    {
        this.card = card;
        this.name = card.name;
        card.ui = this; 

        cardTitle.text = card.name;
        cardOps.text = card.ops.ToString();
        cardText.text = card.cardText;
        cardOps.color = Color.white;

        if (card.Data.image != null)
            cardImage = card.Data.image; 

        if (card.Faction != null)
        {
            factionIcon.color = card.Faction.controlColor;
            cardTitle.color = card.Faction.controlColor; 

            Color.RGBToHSV(card.Faction.controlColor, out float h, out float s, out float v);
            backgroundImage.color = Color.HSVToRGB(h, Mathf.Clamp(s - .4f, 0.1f, 1f), v); 
        }
        else
        {
            factionIcon.color = Color.gray;
            backgroundImage.color = Color.gray; 
            cardTitle.color = Color.black;
            cardOps.color = Color.black; 
        }
    }

    Vector3 beginDragPosition;
    Transform parent; 
    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject; 
        beginDragPosition = transform.position;
        cardText.gameObject.SetActive(false); 
        
        if(TryGetComponent(out CanvasGroup canvas))
        {
            parent = transform.parent;
            transform.SetParent(parent.parent); 
            canvas.blocksRaycasts = false; 
            canvas.alpha = 0.5f;
        }

        cardDragEvent?.Invoke(UI_PlayerBoard.currentPlayer, card);

        /*
        // Move this out - doesn't really belong in the CARD UI -> This has to do a "Display Action Choice Event Trigger" and we link the show of the ActionChoiceUI 
        if(Game.ActionChoice != null)
        {
            foreach (PlayerAction action in availableActions)
            {
                action.SetPlayer(UI_PlayerBoard.currentPlayer); // The player is whichever player's hand we're showing NO MATTER WHERE THE DRAG STARTS
                action.SetCard(card);
            }

            // Do this on every drag start, or simple make the thing visible? For now we create it fresh. 
            if(availableActions.Count(action => action.Can(action.Player, action.Card)) > 0)
            {
                selectionManager = new(availableActions.Where(action => action.Can(action.Player, action.Card)));

                //PlayerAction nextAction;
                await selectionManager.Selection;
                selectionManager.Close();
                
                //await nextAction.Event();
                //Game.SetActionResult(nextAction); // This finally ends a StartActionRound I think.
            } 
        }
        */
    }

    public void OnDrag(PointerEventData eventData) => transform.position += (Vector3)eventData.delta; 

    public void OnEndDrag(PointerEventData eventData)
    {
        if (TryGetComponent(out CanvasGroup canvas))
        {
            canvas.blocksRaycasts = true;
            canvas.alpha = 1f;
            transform.SetParent(parent, false);
        }

        cardEndDragEvent?.Invoke(); 
    }

    public void SetHighlight(Color color)
    {
        highlight.SetActive(true);
        highlight.GetComponent<Image>().color = color; 
    }

    public void ClearHighlight() => highlight.SetActive(false); 

}