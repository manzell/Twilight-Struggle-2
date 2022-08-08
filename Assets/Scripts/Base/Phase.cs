using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System; 
using System.Linq;
using Sirenix.OdinInspector;
using System.Threading.Tasks; 

public class Phase : SerializedMonoBehaviour
{
    Phase parent;
    public event Action phaseStartEvent, phaseEndEvent;
    public static event Action<Phase> PhaseStartEvent, PhaseEndEvent;
    
    public Stack<PlayerAction> cardActions = new(); // These aren't card actions, these are really "Phase actions" 
    public Queue<PhaseAction> onPhaseEvents = new(),
        afterPhaseEvents = new();

    public void Push(PlayerAction cardAction) => cardActions.Push(cardAction); 

    public List<Modifier> modifiers = new();
    public List<Effect> activeEffects = new();

    public async virtual Task DoPhase(Phase parent)
    {
        this.parent = parent;
        Game.currentPhase = this;

        Debug.Log($"Start Phase {name}");

        PhaseStartEvent?.Invoke(this); 
        phaseStartEvent?.Invoke();

        while (onPhaseEvents.Count > 0)
        {
            PhaseAction action = onPhaseEvents.Dequeue(); 
            await action.Do(this);
        }

        while (afterPhaseEvents.Count > 0)
            await afterPhaseEvents.Dequeue().Do(this);

        PhaseEndEvent?.Invoke(this);
        phaseEndEvent?.Invoke();

        if (Next() != null)
            await Next().DoPhase(this);
        else
            Game.EndGame();
    }

    Phase Next()
    {
        List<Phase> children = GetComponentsInChildren<Phase>().Where(p => p != this).ToList();        
        return children.Count > 0 ? children.First() : transform.parent.GetComponent<Phase>()?.Next(this);
    }

    Phase Next(Phase child)
    {
        int i = child.transform.GetSiblingIndex() + 1;

        if (transform.childCount > i)
            return transform.GetChild(i).GetComponent<Phase>(); 
        else
            return transform.parent?.GetComponent<Phase>().Next(this); 
    }

    public static T GetCurrent<T>() => Game.currentPhase.GetComponentInParent<T>();
    public T Get<T>() => GetComponentInParent<T>(); 
}