using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpaceRace : ScriptableObject
{
    public List<SpaceStage> spaceStages = new();
    public Dictionary<Player, int> spaceRaceAttempts = new(); 

    public SpaceStage NextStage(Player player)
    {
        for(int i = 0; i < spaceStages.Count; i++)
            if (spaceStages[i].accomplished.Contains(player)) 
                return spaceStages[i];
        return null; 
    }

    public void Accomplish(SpaceStage stage, Player player)
    {
        if (stage.vpAwards.Length > stage.accomplished.Count)
        {
            player.AdjustVP(stage.vpAwards[stage.accomplished.Count]);
            stage.accomplished.Add(player); 
        }
    }
}
