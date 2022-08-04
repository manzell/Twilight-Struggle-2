using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionManager : MonoBehaviour
{
    System.Action<Card> callback;
    List<UI_Card> selectableCards;
    public List<Card> selectedCards = new();
    public bool cardSelectionComplete { get; private set; } 

    public CardSelectionManager(IEnumerable<Card> availableCards, System.Action<Card> callback)
    {
        selectableCards = new List<UI_Card>();
        this.callback = callback;
    }

    public void CloseSelectionManager()
    {
        foreach(UI_Card card in selectableCards)
        {

        }
    }
}
