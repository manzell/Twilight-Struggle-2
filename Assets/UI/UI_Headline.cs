using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Headline : MonoBehaviour
{
    [SerializeField] GameObject headlineArea;

    public void Show()
    {
        headlineArea.SetActive(true);

        foreach (UI_HeadlineDropHandler handler in headlineArea.GetComponentsInChildren<UI_HeadlineDropHandler>(true))
            handler.Reset();
    }

    public void Hide()
    {
        headlineArea.SetActive(false);
    }
}
