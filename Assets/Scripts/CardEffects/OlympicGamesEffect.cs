using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class OlympicGamesEffect : CardEffect
{
    [SerializeField] int VPAward = 2;
    [SerializeField] int sponsorBonus = 2; 
    public override Task Event(Card card, Player player)
    {
        Debug.Log($"Event({card.name}, {player.name}) received"); 
        Participate(player.enemyPlayer);
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

        (roll > opponentRoll ? opponent.enemyPlayer : opponent).AdjustVP(VPAward); 
    }

    void Boycott(Player opponent)
    {
        Game.AdjustDEFCON(-1);

        // Have to give the enemy a play here 

    }
}
