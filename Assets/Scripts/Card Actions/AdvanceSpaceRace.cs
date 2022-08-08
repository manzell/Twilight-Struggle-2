using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AdvanceSpaceRace : PlayerAction
{
    [SerializeField] int numSteps; 

    protected override Task Action(Player player, Card card)    
    {
        SpaceRace spaceRace = GameObject.FindObjectOfType<SpaceRace>();
        
        for(int i = 0; i < numSteps; i++)
            spaceRace.NextStage(player)?.Accomplish(player);
        
        return Task.CompletedTask; 
    }
}
