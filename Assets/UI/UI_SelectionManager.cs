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

    public void Start()
    {
        SelectionManager<Country>.SelectionStartEvent += StartCountrySelection;
        SelectionManager<Country>.SelectionEndEvent += CloseCountrySelector; 
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

    void SetHighlight(ISelectable country) => ((Country)country).UI.SetHighlight(Color.red);
    void ClearHighlight(ISelectable country) => ((Country)country).UI.ClearHighlight();
}