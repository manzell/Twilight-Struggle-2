using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; 
using System.Threading.Tasks;
using System.Globalization; 

public class UI_ActionSelection : MonoBehaviour
{
    [SerializeField] GameObject actionSelectionPrefab, actionArea;

    public event Action<PlayerAction> activationHandler;

    public TaskCompletionSource<PlayerAction> task = new();
    public Task Task => task.Task; 

    public void Select(PlayerAction p) => activationHandler.Invoke(p); 

    public void Summon(Player player, PlayerAction action)
    {
        actionArea.SetActive(true); 
        UI_ActionSelector selector = Instantiate(actionSelectionPrefab, actionArea.transform).GetComponent<UI_ActionSelector>();

        selector.Setup(this, action);
        if (player != null)
            selector.requiredPlayer = player;         
    }

    public void Dismiss(PlayerAction p)
    {
        foreach(Transform child in actionArea.transform)
            if (child.GetComponent<UI_ActionSelector>().Action == p) // Considering using GetType because the actions might not still match
                Destroy(child.gameObject);

        actionArea.SetActive(false);
    }
}