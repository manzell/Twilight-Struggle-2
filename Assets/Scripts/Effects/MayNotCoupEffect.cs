using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayNotCoupEffect : Effect
{
    [SerializeField] Faction faction;
    public override bool Test(PlayerAction action) => !(action is Coup && action.Player.faction == faction); 
}
