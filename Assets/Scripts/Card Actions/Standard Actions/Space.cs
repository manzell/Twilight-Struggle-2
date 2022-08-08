using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class Space : PlayerAction
{
    [SerializeField] SpaceRace spaceRace; 
    public override bool Can(Player player, Card card)
    {
        spaceRace = GameObject.FindObjectOfType<SpaceRace>();
        SpaceStage stage = spaceRace.NextStage(player);

        return card.ops + modifier >= stage.requiredOps &&
            spaceRace.spaceRaceAttemptsMade.Count(attempt => attempt.Value.player == player) < spaceRace.spaceRaceNumAttempts[player];             
    }

    protected override Task Action(Player player, Card card)
    {
        SpaceStage stage = spaceRace.NextStage(player);
        SpaceAttempt attempt = new(stage, player, card);

        spaceRace.spaceRaceAttemptsMade.Add(player, attempt);
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
        public bool successful => roll.Value <= stage.requiredRoll;

        public SpaceAttempt(SpaceStage stage, Player player, Card card)
        {
            this.stage = stage;
            this.player = player;
            this.card = card;
            roll = new Roll(0);
        }
    }
}
