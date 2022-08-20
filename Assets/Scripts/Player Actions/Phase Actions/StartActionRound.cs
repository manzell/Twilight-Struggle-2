using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class StartActionRound : PhaseAction
{
    [SerializeField] PlayCard playAction;
    TaskCompletionSource<PlayerAction> actionTCS;

    public async override Task Do(Phase phase)
    {
        actionTCS = new(); 
        (phase as ActionRound).SetActionRoundStart(this);

        UI_Card.cardDragEvent += onCardDrag;
        UI_Card.cardEndDragEvent += onEndCardDrag; 
        twilightStruggle.UI.UI_Message.SetMessage($"Play {(phase as ActionRound).phasingPlayer.name} Action Round");

        await actionTCS.Task;

        UI_Card.cardDragEvent -= onCardDrag;
        UI_Card.cardEndDragEvent -= onEndCardDrag;
    }

    public async void onCardDrag(Player player, Card card)
    {
        playAction.SetCard(card);
        playAction.SetPlayer(player);

        await playAction.Event();

        actionTCS.SetResult(playAction); 
    }

    public void onEndCardDrag()
    {
        playAction.Cancel();
    }
}
