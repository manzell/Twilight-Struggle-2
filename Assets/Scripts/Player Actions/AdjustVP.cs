using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AdjustVP : PlayerAction
{
    [SerializeField] Faction faction;
    [SerializeField] int vpAmount;

    public override Task Action()
    {
        (faction.player ?? Player).AdjustVP(vpAmount);
        return Task.CompletedTask; 
    }
}
