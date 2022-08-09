using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System.Threading.Tasks;

public class PlayCard : PlayerAction
{
    public List<PlayerAction> Actions => availableActions; 
    [SerializeField] List<PlayerAction> availableActions = new();
    [SerializeField] Card requiredCard;
    bool opponentEventTriggered; 

    protected override async Task Action()
    {
        twilightStruggle.UI.UI_Message.SetMessage($"Play {Player.name} Action Round");
        
        SelectionManager<PlayerAction> selectionManager = new(availableActions);
        PlayerAction playerAction;

        while((playerAction = await selectionManager.Selection).Card != requiredCard && requiredCard != null)
            continue;

        selectionManager.Close();
        
        Debug.Log($"{Player.name} choooses {playerAction} with {playerAction.Card.name}");
        
        await playerAction.Event(Player);

        Debug.Log($"Ending {playerAction.Card.name} Play");

        if (playerAction.Card.Faction == Player.enemyPlayer.faction && opponentEventTriggered == false)
        {
            if(playerAction is TriggerEvent)
            {
                opponentEventTriggered = true;
                PlayCard nextChoice = (PlayCard)this.Clone();
                nextChoice.RemoveAction(playerAction.GetType());
                nextChoice.requiredCard = playerAction.Card; 
                Debug.Log($"Evented. {Player.name} gets to use {playerAction.Card.name} for Ops");
                if (nextChoice.requiredCard != null)
                    Debug.Log($"Must use {playerAction.Card.name}?");
                await nextChoice.Event(this); 
            }
            else if(playerAction is not Space)
            {
                Debug.Log("Triggering Opponent Event");
                await playerAction.Card.Event(Player); 
            }
        }

    }

    public void RemoveAction(System.Type actionToRemove)
    {
        PlayerAction[] actions = availableActions.ToArray();

        foreach(PlayerAction action in actions)
            if (action.GetType() == actionToRemove)
                availableActions.Remove(action); 
    }
}
