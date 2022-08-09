using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//[CreateAssetMenu(menuName = "CardEffect/Adjust VP")]
public class AdjustVP : PlayerAction
{
    [SerializeField] Faction faction;
    [SerializeField] int vpAmount;

    protected override Task Action()
    {
        (Player ?? Card.Faction.player).AdjustVP(vpAmount);

        return Task.CompletedTask; 
    }
}
