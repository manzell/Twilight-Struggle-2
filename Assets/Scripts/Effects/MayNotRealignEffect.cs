using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class MayNotRealignEffect : Effect
    {
        [SerializeField] Faction faction;
        public override bool Test(PlayerAction action) => !(action is Realign && action.Player.Faction == faction);
    }
}