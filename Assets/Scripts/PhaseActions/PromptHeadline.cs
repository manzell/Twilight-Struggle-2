using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System.Threading.Tasks;

public class PromptHeadline : PhaseAction
{
    [SerializeField] UI_Headline ui_headline;

    public override async Task Do(Phase phase)
    {
        if (phase is HeadlinePhase headlinePhase)
        {
            Debug.Log("Prompting Headline");
            ui_headline.Show();

            while (headlinePhase.headlines_set == false)
                await Task.Yield();

            Headline headline = new Headline();
            Dictionary<Player, Card> headlines = headlinePhase.Headlines;
            List<Player> headlineOrder = headlines.Keys.OrderByDescending(player => headlines[player].ops).ThenBy(p => p.faction.name).ToList();

            for(int i = 0; i < headlineOrder.Count; i++)
            {
                Player player = headlineOrder[i];
                Card headlineCard = headlines[player];

                await headlineCard.Headline(player);
            }
        }
    }
}
