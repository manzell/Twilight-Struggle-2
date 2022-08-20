using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class UI_Defcon : MonoBehaviour
{
    [SerializeField] RawImage background;
    [SerializeField] Outline border;
    [SerializeField] TextMeshProUGUI defcon;
    [SerializeField] AudioClip degradeSound, upgradeSound; 

    private void Awake()
    {
        Game.OnDefconAdjust += OnDefconAdjust; 
    }

    public void OnDefconAdjust(int x)
    {
        defcon.text = Game.DEFCON.ToString(); 
    }
}
