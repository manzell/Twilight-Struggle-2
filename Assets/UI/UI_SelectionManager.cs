using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq; 

public class UI_SelectionManager : MonoBehaviour
{
    [SerializeField] GameObject actionSelectionPrefab, cardPrefab, actionArea;
    SelectionManager<PlayerAction> actionSelectionManager; 

    public void Start()
    {
        SelectionManager<PlayerAction>.SelectionStartEvent += OpenActionChoice;
        SelectionManager<PlayerAction>.SelectionEndEvent += CloseActionChoice;
        SelectionManager<Country>.SelectionStartEvent += StartCountrySelection;
        SelectionManager<Country>.SelectionEndEvent += CloseCountrySelector; 
    }

    public void OpenActionChoice(SelectionManager<PlayerAction> selectionManager)
    {
        actionSelectionManager = selectionManager;
        selectionManager.addSelectableEvent += ShowPlayerAction;
        selectionManager.removeSelectableEvent += RemovePlayerAction;
        actionArea.SetActive(true);
    }

    public void CloseActionChoice(SelectionManager<PlayerAction> selectionManager)
    {
        actionArea.SetActive(false);

        foreach (Transform t in actionArea.transform)
            Destroy(t.gameObject);
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

    void ShowPlayerAction(ISelectable playerAction)
    {
        UI_PlayerAction uiPlayerAction = Instantiate(actionSelectionPrefab, actionArea.transform).GetComponent<UI_PlayerAction>();
        uiPlayerAction.Setup(playerAction as PlayerAction, actionSelectionManager);
    }

    void RemovePlayerAction(ISelectable playerAction)
    {
    }

    void SetHighlight(ISelectable country) => ((Country)country).UI.SetHighlight(Color.red);
    void ClearHighlight(ISelectable country) => ((Country)country).UI.ClearHighlight();

    public void DestroyPlayerAction(ISelectable playerAction)
    {
        foreach (Transform child in actionArea.transform)
            if (child.GetComponent<UI_PlayerAction>()?.Action == playerAction) // This is failing because Cards don't have UI_PlayerAction. But they are ISelectable
                Destroy(child.gameObject);
    }
}