using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System.Threading.Tasks;

public class PromptHeadline : PhaseAction
{
    [SerializeField] UI_Headline ui_headline;
    [SerializeField] Faction USA, USSR; 

    public override async Task Do(Phase phase)
    {
        if (phase is HeadlinePhase headlinePhase)
        {
            twilightStruggle.UI.UI_Message.SetMessage($"Headline Phase");
            ui_headline.Show();

            foreach (Player player in Game.Players)
                headlinePhase.headlineTasks.Add(player, new TaskCompletionSource<Card>());

            await Task.WhenAll(headlinePhase.headlineTasks.Values.Select(task => task.Task));

            ui_headline.Hide();

            // We check for a CancelHeadline headlineEffect

            if(headlinePhase.headlines[USA.player].Data.headlineActions.All(ha => !(ha is CancelHeadline && ha.Card.Faction == USA)))
            {
                foreach (Player player in headlinePhase.headlines.Keys.OrderByDescending(player => headlinePhase.headlines[player].ops).ThenBy(p => p.faction.name))
                {
                    Debug.Log($"Headlining {headlinePhase.headlines[player].name} for {player.name}");
                    await headlinePhase.headlines[player].Headline(player);
                }
            }
            else
            {
                Debug.Log("Canceling Headline");
            }
        }
    }
}
