using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonusOpModifier : Modifier
{
    [SerializeField] Player player;

    public override bool Applies(GameAction gameAction)
    {
        if (amount > 0 && gameAction.card.ops >= max) return false;
        if (amount < 0 && gameAction.card.ops <= min) return false; 
        if (gameAction.player != player) return false;

        return true; 
    }
}
