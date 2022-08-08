using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Modifier
{
    public string name;
    public int amount, min, max;

    public virtual bool Applies(PlayerAction gameAction) => true; 
}
