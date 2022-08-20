using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System.Threading.Tasks;

public class PlayCard : PlayerAction
{
    public IEnumerable<PlayerAction> Actions => availableActions; 
    [SerializeField] List<PlayerAction> availableActions;
    [SerializeField] Card requiredCard;
    bool opponentEventTriggered;
    SelectionManager<PlayerAction> selectionManager;

    public PlayCard() { }
    public PlayCard(IEnumerable<PlayerAction> availableActions, Card requiredCard = null)
    {
        this.availableActions = availableActions.ToList();
        this.requiredCard = requiredCard; 
    }

    public void Cancel() => selectionManager?.Close(); 
    public override void SetPlayer(Player player)
    {
        foreach (var action in availableActions)
            action.SetPlayer(player);
        base.SetPlayer(player);
    }

    public override async Task Action()
    {
        selectionManager = new(availableActions.Where(action => action.Can(Player, Card))); // RIGHT NOW: PLAYCARD drop is not receiving the card drop action. Because card drop is not setting the result of the selectionManager.Selection
        PlayerAction selectedAction = await selectionManager.Selection;
        selectionManager.Close();

        selectedAction.Select();
        await selectedAction.Event();
        /* PLAY LOGIC:
         * 
         * If it's a friendly card, we're finish the Action, nothing to worry about. 
         * if we've already triggered OpponentEvent, we're also finish the Action
         * If we triggered an Opponent Event, let's mark opponenetEventTriggered = True and then present them a new PlayCard action with everything but action we just triggered
         * If we didn't trigger the opponent event, trigger it now, then end the Action.
         */
        if (selectedAction.Card.Faction == Player.Faction.enemyFaction && opponentEventTriggered == false)
        {
            if (selectedAction is TriggerEvent && opponentEventTriggered == false)
            {
                Debug.Log($"Just triggered Opponents event. Play the {selectedAction.Card} for it's Ops Value ({selectedAction.Card.ops})");
                opponentEventTriggered = true;

                PlayCard newPlayCard = new(availableActions.Where(action => action != selectedAction));
                await newPlayCard.Event();
            }
            else if (selectedAction is Space)
            {
                Debug.Log("Card Spaced. Moving On");
            }
            else if (!(selectedAction is Space) && opponentEventTriggered == false)
            {
                Debug.Log($"Opponent Card Played. Triggering {selectedAction.Card}");
                await Card.Event(Player);
            }
        }

        if (selectedAction.Card.Data.removeOnEvent)
        {
            Debug.Log($"{selectedAction.Card.name} removed from the Game.");
            Game.removed.Add(selectedAction.Card);
        }
        else
        {
            Debug.Log($"{selectedAction.Card.name} discarded.");
            Game.discards.Add(selectedAction.Card);
        }
    }

    public void RemoveAction(System.Type actionToRemove)
    {
        PlayerAction[] actions = availableActions.ToArray();

        foreach (PlayerAction action in actions)
            availableActions = availableActions.Where(action => action.GetType() != actionToRemove).ToList(); 
    }
}
