using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class Space : PlayerAction
{
    [SerializeField] SpaceRace spaceRace;

    protected override Task Action()
    {
        SpaceStage stage = spaceRace.NextStage(Player);
        SpaceAttempt attempt = new(stage, Player);

        spaceRace.spaceRaceAttemptsMade.Add(attempt);
        if (attempt.successful)
            stage.Accomplish(Player);

        return Task.CompletedTask;
    }

    public override bool Can(Player player, Card card)
    {
        if (card == null || card.Data is ScoringCard) return false;
        else return modifiedOpsValue >= spaceRace.NextStage(player).requiredOps &&
            spaceRace.spaceRaceAttemptsMade.Count(attempt => attempt.player == player && attempt.turn == Phase.GetCurrent<Turn>()) < spaceRace.spaceRaceTurnAttemptLimit[player];
    }

    public void SetSpaceRace(SpaceRace spaceRace) => this.spaceRace = spaceRace;
    public SpaceRace GetSpaceRace() => spaceRace;

    public class SpaceAttempt
    {
        public SpaceStage stage;
        public Roll roll;
        public Player player;
        public Turn turn; 
        public bool successful => roll.Value <= stage.requiredRoll;

        public SpaceAttempt(SpaceStage stage, Player player)
        {
            this.stage = stage;
            this.player = player;
            
            roll = new Roll(0);
            turn = Phase.GetCurrent<Turn>(); 

            Debug.Log($"{player.name} attempts {stage.name}. {player.name} rolls a {roll.Value} and {(this.successful ? "succeeds" : "fails")}.");
        }
    }
}
