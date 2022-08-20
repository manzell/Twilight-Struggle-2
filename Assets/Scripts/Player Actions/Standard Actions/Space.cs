using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq;
using System.Threading.Tasks;

public class Space : PlayerAction
{
    public static event Action<SpaceAttempt> prepareSpaceAttempt, spaceAttempt;
    SpaceRace spaceRace => GameObject.FindObjectOfType<SpaceRace>(); 

    public async override Task Action()
    {
        SpaceStage stage = spaceRace.NextStage(Player);
        SpaceAttempt attempt = new(stage, Player);

        spaceRace.spaceRaceAttemptsMade.Add(attempt);
        if (attempt.successful)
            stage.Accomplish(Player);

        await attempt.spaceCompletion.Task;
    }

    public override bool Can(Player player, Card card)
    {
        if (card == null || card.Data is ScoringCard) return false;

        Card oldCard = Card;
        SetCard(card);

        bool retval = modifiedOpsValue >= spaceRace.NextStage(player).RequiredOps &&
            spaceRace.spaceRaceAttemptsMade.Count(attempt => attempt.player == player && attempt.turn == Phase.GetCurrent<Turn>()) < spaceRace.spaceRaceTurnAttemptLimit[player];

        SetCard(oldCard);
        return retval; 
    }

    public class SpaceAttempt
    {
        public TaskCompletionSource<SpaceAttempt> spaceCompletion = new();
        public SpaceStage stage;
        public Roll roll;
        public Player player;
        public Turn turn; 
        public bool successful => roll.Value <= stage.RequiredRoll;

        public SpaceAttempt(SpaceStage stage, Player player)
        {
            prepareSpaceAttempt?.Invoke(this); 
            this.stage = stage;
            this.player = player;
            
            roll = new Roll(0);
            turn = Phase.GetCurrent<Turn>(); 

            Debug.Log($"{player.name} attempts {stage.name}. {player.name} rolls a {roll.Value} and {(this.successful ? "succeeds" : "fails")}.");
            spaceAttempt?.Invoke(this); 
        }
    }
}
