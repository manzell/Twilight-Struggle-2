using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Prevent Effect")]
public class PreventEffect : PlayerAction
{
    //[SerializeField] OngoingEffect effect; 

    protected override Task Action(Player player, Card card)
    {
        //throw new System.NotImplementedException();
        return Task.CompletedTask; 
    }
}
