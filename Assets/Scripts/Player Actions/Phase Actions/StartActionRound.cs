using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class StartActionRound : PhaseAction
{
    [SerializeField] PlayCard playCardAction;
    TaskCompletionSource<PlayerAction> actionRoundTaskSource;

    public async override Task Do(Phase phase)
    {
        actionRoundTaskSource = new(); 
        (phase as ActionRound).SetActionRoundStart(this);

        UI_Card.cardDragEvent += onCardDrag;
        UI_Card.cardEndDragEvent += onEndCardDrag; 
        twilightStruggle.UI.UI_Message.SetMessage($"Play {(phase as ActionRound).phasingPlayer.name} Action Round");

        PlayerAction playerAction = await actionRoundTaskSource.Task;

        UI_Card.cardDragEvent -= onCardDrag;
        UI_Card.cardEndDragEvent -= onEndCardDrag;
    }

    public async void onCardDrag(Player player, Card card)
    {
        playCardAction.SetCard(card);
        playCardAction.SetPlayer(player);

        await playCardAction.Event();

        actionRoundTaskSource.SetResult(playCardAction); 
    }

    public void onEndCardDrag()
    {
        playCardAction.Cancel();
    }
}
