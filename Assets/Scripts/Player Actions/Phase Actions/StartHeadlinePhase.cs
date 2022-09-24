using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System.Threading.Tasks;

public class StartHeadlinePhase : PhaseAction
{
    public override async Task Do(Phase phase)
    {
        if (phase is HeadlinePhase headlinePhase)
        {
            List<PlayerAction> headlines = new();
            twilightStruggle.UI.UI_Message.SetMessage($"Headline Phase");

            foreach (Player player in Game.Players)
            {
                Headline headline = new();
                headline.SetPlayer(player);
                headlines.Add(headline); 
            }

            // Create UI Stuff

            // Check for any Cancel Headline Effects. Only a non-enemy card headlined should return here. 
            /*
            List<Card> cancelHeadlineCards = headlines.Where(_HeadlineFilter).Select(action => action.Card).ToList(); 

            bool _HeadlineFilter(PlayerAction headline)
            {
                bool friendlyCard = card.Faction != headline.Player.Enemy.Faction; 
                bool cancelsHeadlines = card.Data.headlineActions.Any(headlineAction => headlineAction is CancelHeadline);
                return friendlyCard && cancelsHeadlines; 
            }

            if (cancelHeadlineCards.Count == 0)
            {
                foreach (PlayerAction headline in headlines.OrderByDescending(headline => headline.Card.ops).ThenBy(headline => headline.Player.name))
                {
                    twilightStruggle.UI.UI_Message.SetMessage($"Headlining {headline.Card.name} for {headline.Player.name}");
                    Game.InvokePlayEvent(headline.Player, headline.Card); // Questionable    

                    await headline.Action(); 
                }
            }
            else
            {
                Debug.Log($"Headlines cancelled by {cancelHeadlineCards.First().name}");
                foreach(Headline headline in headlines)
                    Game.discards.Add(headline.Card); 
            }
            */
        }
    }
}
