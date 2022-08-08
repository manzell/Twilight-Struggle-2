using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System.Threading.Tasks;

public class PromptActionRound : PhaseAction
{
    [SerializeField] List<PlayerAction> availableActions = new();

    public override async Task Do(Phase phase)
    {
        SelectionManager<PlayerAction> selectionManager = new(availableActions);
        twilightStruggle.UI.UI_Message.SetMessage($"Play {(phase as ActionRound).phasingPlayer.name} Action Round");
        PlayerAction playerAction = await selectionManager.Selection;
        selectionManager.Close();

        await playerAction.Event((phase as ActionRound).phasingPlayer); 
    }
}
