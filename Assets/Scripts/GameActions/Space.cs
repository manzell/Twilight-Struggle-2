using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

[CreateAssetMenu]
public class Space : GameAction
{
    public override bool Can(Player player, Card card)
    {
        SpaceRace spaceRace = GameObject.FindObjectOfType<SpaceRace>();
        SpaceStage stage = spaceRace.NextStage(player);

        return card.ops + modifier >= stage.requiredOps &&
            Game.currentPhase.GetCurrent<Turn>().spaceAttempts.Count(attempt => attempt.player == player) < spaceRace.spaceRaceAttempts[player];             
    }

    public override Task Event(Player player, Card card)
    {
        this.player = player;
        this.card = card;

        SpaceStage stage = GameObject.FindObjectOfType<SpaceRace>().NextStage(player);
        SpaceAttempt attempt = new(stage, player, card);

        Game.currentPhase.GetCurrent<Turn>().spaceAttempts.Add(attempt);
        if (attempt.successful)
            stage.Accomplish(player);

        return Task.CompletedTask; 
    }

    public class SpaceAttempt
    {
        public SpaceStage stage;
        public Roll roll;
        public Player player;
        public Card card;
        public bool successful;

        public SpaceAttempt(SpaceStage stage, Player player, Card card)
        {
            this.stage = stage;
            this.player = player;
            this.card = card;
            roll = new Roll(0);

            successful = roll.Value <= stage.requiredRoll;
        }
    }
}
