using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System; 
using TMPro;
using System.Threading.Tasks; 

public class UI_HeadlineDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField] Faction faction;
    [SerializeField] TextMeshProUGUI cardName, header;

    public static event Action<Card> headlineEvent;

    void Start()
    {
        headlineEvent += RemoveFromHandOnHeadline; 
    }

    public void Reset()
    {
        cardName.enabled = false; 
        header.enabled = true;
    }

    public async void OnDrop(PointerEventData eventData)
    {
        if (eventData.selectedObject.TryGetComponent(out UI_Card uiCard))
        {
            HeadlinePhase headlinePhase = Game.currentPhase.GetCurrent<HeadlinePhase>();
            Player currentPlayer = FindObjectOfType<UI_Hand>().currentPlayer;
            Card card = uiCard.card;

            if (currentPlayer.faction == faction && currentPlayer.hand.Contains(card) && headlinePhase.GetHeadline(currentPlayer) == null)
            {
                cardName.text = card.name;
                cardName.enabled = true;
                header.enabled = false;

                Headline headline = new();
                await headline.Event(currentPlayer, card); 
                headlinePhase.AddHeadline(headline);
                headlineEvent.Invoke(card); 
            }
        }
    }

    void RemoveFromHandOnHeadline(Card card)
    {
        FindObjectOfType<UI_Hand>().RemoveCardFromHand(card);
    }
}
