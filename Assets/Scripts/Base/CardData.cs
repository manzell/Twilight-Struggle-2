using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector; 

public class CardData : SerializedScriptableObject
{
    public new string name; 
    public bool removeOnEvent; 
    public Faction faction; 
    public int opsValue;
    [Multiline] public string cardText;
    public List<PlayerAction> playActions = new(), 
        headlineActions = new();

    public Image image;
    public UI_Card card;
}
