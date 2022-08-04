using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NATOEffect : Effect
{
    [SerializeField] Player USA, USSR;
    Country country; 

    public override bool Test(GameAction action) => !((action is Coup || action is Realign) && country.control == USA && action.player == USSR);
}
