using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeadlinePhase : Phase
{
    [SerializeField] Dictionary<Player, Card> headlines = new();
    [SerializeField] List<Headline> headlinePlays = new();

    public bool headlines_set => headlines.Count == 2;
    public Dictionary<Player, Card> Headlines => new(headlines); 

    public Card GetHeadline(Player player) => headlines.ContainsKey(player) ? headlines[player] : null;
    public void AddHeadline(Headline headline)
    {
        headline.player.hand.Remove(headline.card); // Move this to an event listener?
        headlines.Add(headline.player, headline.card);
        this.Continue();
    }
}
