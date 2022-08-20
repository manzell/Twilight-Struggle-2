using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AdvanceSpaceRace : PlayerAction
{
    [SerializeField] int numSteps;

    public override Task Action()    
    {
        SpaceRace spaceRace = GameObject.FindObjectOfType<SpaceRace>();
        
        for(int i = 0; i < numSteps; i++)
            spaceRace.NextStage(Player)?.Accomplish(Player);
        
        return Task.CompletedTask; 
    }
}
