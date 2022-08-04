using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ActionRound : MonoBehaviour
{
    [SerializeField] GameObject actionRoundArea;

    public void Show()
    {
        actionRoundArea.SetActive(true);
    }

    public void Hide()
    {
        actionRoundArea.SetActive(false);
    }
}
