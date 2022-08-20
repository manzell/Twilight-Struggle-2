using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UI_SelectionManager : MonoBehaviour
{
    [SerializeField] GameObject actionSelectionPrefab, cardPrefab, actionArea;

    public void Start()
    {
        SelectionManager<PlayerAction>.SelectionStartEvent += OpenActionChoice;
        SelectionManager<PlayerAction>.SelectionEndEvent += CloseActionChoice;
        SelectionManager<Country>.SelectionStartEvent += StartCountrySelection;
        SelectionManager<Country>.SelectionEndEvent += CloseCountrySelector; 
    }

    public void OpenActionChoice(SelectionManager<PlayerAction> selectionManager)
    {
        foreach (Transform t in actionArea.transform)
            Destroy(t.gameObject); 

        actionArea.SetActive(true);
        selectionManager.addSelectableEvent += CreatePlayerAction;
        selectionManager.removeSelectableEvent += DestroyPlayerAction;
    }

    public void CloseActionChoice(SelectionManager<PlayerAction> selectionManager)
    {
        actionArea.SetActive(false);
        selectionManager.addSelectableEvent -= CreatePlayerAction;
        selectionManager.removeSelectableEvent -= DestroyPlayerAction;
    }

    public void StartCountrySelection(SelectionManager<Country> selectionManager)
    {
        selectionManager.addSelectableEvent += SetHighlight;
        selectionManager.removeSelectableEvent += ClearHighlight;
    }

    public void CloseCountrySelector(SelectionManager<Country> selectionManager)
    {
        selectionManager.addSelectableEvent -= SetHighlight;
        selectionManager.removeSelectableEvent -= ClearHighlight;
    }

    void SetHighlight(ISelectable country) => ((Country)country).UI.SetHighlight(Color.red);
    void ClearHighlight(ISelectable country) => ((Country)country).UI.ClearHighlight(); 

    public void UISelect(PlayerAction playerAction)
    {
        // TODO: Let's use this card in a better way
        UI_Card uiCard = Instantiate(cardPrefab, actionArea.transform).GetComponent<UI_Card>();
        uiCard.Setup(playerAction.Card); 
    }

    public void CreatePlayerAction(ISelectable action)
    {
        if(action is PlayerAction)
            Instantiate(actionSelectionPrefab, actionArea.transform).GetComponent<UI_PlayerAction>().Setup(this, (PlayerAction)action);
    }

    public void DestroyPlayerAction(ISelectable playerAction)
    {
        foreach (Transform child in actionArea.transform)
            if (child.GetComponent<UI_PlayerAction>()?.Action == playerAction) // This is failing because Cards don't have UI_PlayerAction. But they are ISelectable
                Destroy(child.gameObject);
    }
}