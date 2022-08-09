using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class SpaceRace : MonoBehaviour
{
    public List<SpaceStage> spaceStages = new();
    public Dictionary<Player, int> spaceRaceNumAttempts = new();
    public List<Space.SpaceAttempt> spaceRaceAttemptsMade = new();

    public SpaceStage NextStage(Player player) => 
        spaceStages.First(stage => spaceRaceAttemptsMade.Count(attempt => attempt.player == player && attempt.successful == true && attempt.stage == stage) == 0); 
}
