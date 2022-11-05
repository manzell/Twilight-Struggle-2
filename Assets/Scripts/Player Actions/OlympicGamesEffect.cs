using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class OlympicGamesEffect : PlayerAction
    {
        [SerializeField] int VPAward = 2;
        [SerializeField] int sponsorBonus = 2;
        public override Task Action()
        {
            TwilightStruggle.UI.Message.SetMessage($"{Player.name} hosts the Olympics. {Player.Enemy.name}, Participate or Boycott?");
            Participate(Player.Enemy);
            // Prompt Opponent to Participate or Boycott
            // For now we'll implement Boycott but they'll be no way to trigger it. 

            return Task.CompletedTask;
        }

        void Participate(Player opponent)
        {
            int roll = 0;
            int opponentRoll = 0;

            while (roll == opponentRoll)
            {
                roll = Random.Range(0, 6) + 1 + sponsorBonus;
                opponentRoll = Random.Range(0, 6) + 1;
            }

            Player winningPlayer = roll > opponentRoll ? opponent.Enemy : opponent;
            int winningRoll = Mathf.Max(roll, opponentRoll);

            TwilightStruggle.UI.Message.SetMessage($"{winningPlayer.name} wins {winningRoll} Gold Medals to {winningPlayer.Enemy.name}'s {Mathf.Min(roll, opponentRoll)}.");
            winningPlayer.AdjustVP(VPAward);
        }

        void Boycott(Player opponent)
        {
            Game.AdjustDEFCON(-1);

            // Have to give the enemy a play here 

        }
    }
}