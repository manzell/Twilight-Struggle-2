using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector; 

public class SpaceRace : SerializedMonoBehaviour
{
    public List<SpaceStage> spaceStages = new();
    public Dictionary<Player, int> spaceRaceTurnAttemptLimit = new();
    public List<Space.SpaceAttempt> spaceRaceAttemptsMade = new();

    public SpaceStage NextStage(Player player) => 
        spaceStages.First(stage => spaceRaceAttemptsMade.Count(attempt => attempt.player == player && attempt.successful == true && attempt.stage == stage) == 0); 
}
