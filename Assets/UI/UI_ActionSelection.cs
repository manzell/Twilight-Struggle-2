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
    [SerializeField] Player player;
    [SerializeField] GameObject actionSelectionPrefab, actionArea;

    public event Action<PlayerAction> onClickHandler;

    public TaskCompletionSource<PlayerAction> task = new();
    public Task Task => task.Task; 

    public void Select(PlayerAction p)
    {
        Debug.Log($"We just dropped {p.Card.name} onto { p.GetType()}");
    }

    public void Summon(PlayerAction p)
    {
        UI_ActionSelector selector = Instantiate(actionSelectionPrefab, actionArea.transform).GetComponent<UI_ActionSelector>();
        selector.Setup(this, p); 
    }
}