using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Prevent Effect")]
public class PreventEffect : CardEffect
{
    //[SerializeField] OngoingEffect effect; 

    public override Task Event(Card card, Player player)
    {
        //throw new System.NotImplementedException();
        return Task.CompletedTask; 
    }
}
