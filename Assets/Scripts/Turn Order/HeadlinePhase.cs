using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks; 

public class HeadlinePhase : Phase
{
    public Dictionary<Player, TaskCompletionSource<Card>> headlineTasks = new();
    public Dictionary<Player, Card> headlines = new();

    public Card GetHeadline(Player player) => headlines.ContainsKey(player) ? headlines[player] : null; 
    public void SetHeadline(Player player, Card card) 
    { 
        headlines.Add(player, card);
        headlineTasks[player].SetResult(card); 
    }
}
