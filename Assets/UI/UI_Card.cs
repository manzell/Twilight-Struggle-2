using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Sirenix.OdinInspector; 

public class UI_Card : SerializedMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IHighlightable, IPointerClickHandler
{
    public Card card;
    [SerializeField] TextMeshProUGUI cardTitle, cardText, cardOps;
    [SerializeField] Image backgroundImage, cardImage, factionIcon;
    [SerializeField] GameObject highlight;
    [SerializeField] List<PlayerAction> availableActions;
    public SelectionManager<PlayerAction> selectionManager { get; private set; }

    public event Action<Card> onClickHandler;
    public void OnPointerClick(PointerEventData eventData) => onClickHandler?.Invoke(card);

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
    public async void OnBeginDrag(PointerEventData eventData)
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

        // Need a way to determine if a Selection Manager is open
        // Maybe route this through the Game/Phase ActionTask Completion Source? 
        // Task is AWAITED by StartActionRound, maybe that can turn on the task awaiting; 
        if(Phase.GetCurrent<ActionRound>() != null)
        {
            foreach (PlayerAction action in availableActions)
            {
                action.SetCard(card);
                action.SetPlayer(FindObjectOfType<UI_Hand>().currentPlayer);
            }

            selectionManager = new(availableActions);
            PlayerAction p = await selectionManager.Selection;
            selectionManager.Close();
            await p.Event();
            Game.actionChoice.SetResult(p);
        }
    }

    public void OnDrag(PointerEventData eventData) => transform.position += (Vector3)eventData.delta; 

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionManager?.Close(); 

        if (TryGetComponent(out CanvasGroup canvas))
        {
            canvas.blocksRaycasts = true;
            canvas.alpha = 1f;
            transform.SetParent(parent, false); 
        }
    }

    public void SetHighlight(Color color)
    {
        highlight.SetActive(true);
        highlight.GetComponent<Image>().color = color; 
    }

    public void ClearHighlight() => highlight.SetActive(false); 

}