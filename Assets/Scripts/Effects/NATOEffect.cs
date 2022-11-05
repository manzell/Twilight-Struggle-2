using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class NATOEffect : Effect
    {
        [SerializeField] Faction USA, USSR;
        Country country;

        public override bool Test(PlayerAction action) => !((action is Coup || action is Realign) && country.Control == USA && action.Player.Faction == USSR);
    }
}