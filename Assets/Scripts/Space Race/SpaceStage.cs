using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class SpaceStage : ScriptableObject
{
    public int[] vpAwards;
    public int requiredOps;
    public int requiredRoll;

    public List<Player> accomplished { get; private set; } = new();

    public void Accomplish(Player player)
    {
        if (vpAwards.Length > accomplished.Count)
            player.AdjustVP(vpAwards[accomplished.Count]); 

        if(accomplished.Count == 0)
        {
            // Give the player the Power
        }
        else if(accomplished.Count == 1)
        {
            // Remove the power from player 1
        }

        accomplished.Add(player); 
    }
}
