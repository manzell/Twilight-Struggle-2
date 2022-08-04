using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class PreventHeadline : CardEffect
{
    public override bool Can(Card card, Player player) => false;

    public override Task Event(Card card, Player player)
    {
        //throw new System.NotImplementedException();
        return Task.CompletedTask; 
    }
}