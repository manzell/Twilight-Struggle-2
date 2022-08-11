using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonusOpModifier : Modifier
{
    [SerializeField] Faction faction;

    public override bool Applies(PlayerAction gameAction)
    {
        if (amount > 0 && gameAction.Card.ops >= max) return false;
        if (amount < 0 && gameAction.Card.ops <= min) return false; 
        if (gameAction.Player.Faction != faction) return false;

        return true; 
    }
}
