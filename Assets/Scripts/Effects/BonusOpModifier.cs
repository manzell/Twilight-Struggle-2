using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonusOpModifier : Modifier
{
    [SerializeField] Faction faction;

    public override bool Applies(PlayerAction gameAction)
    {
        return true; 
    }
}
