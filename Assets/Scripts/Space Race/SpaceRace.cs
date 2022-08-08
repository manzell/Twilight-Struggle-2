using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRace : MonoBehaviour
{
    public List<SpaceStage> spaceStages = new();
    public Dictionary<Player, int> spaceRaceNumAttempts = new();
    public Dictionary<Player, Space.SpaceAttempt> spaceRaceAttemptsMade = new();

    public SpaceStage NextStage(Player player)
    {
        for(int i = 0; i < spaceStages.Count; i++)
            if (spaceStages[i].accomplished.Contains(player)) 
                return spaceStages[i];
        return null; 
    }
}
