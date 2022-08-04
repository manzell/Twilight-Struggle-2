using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System; 
using System.Linq;
using Sirenix.OdinInspector; 

public class Phase : SerializedMonoBehaviour
{
    Phase parent;
    public event Action phaseStartEvent, phaseEndEvent;
    public static event Action<Phase> PhaseStartEvent, PhaseEndEvent;
    
    public List<GameAction> gameActions = new();
    public Queue<PhaseAction> onPhaseEvents = new(),
        afterPhaseEvents = new();

    public List<Modifier> modifiers = new(); 

    public virtual void Continue()
    {
        /*if (onPhaseEvents.Count > 0)
            onPhaseEvents.Dequeue().Do(this);
        else*/ if (afterPhaseEvents.Count > 0)
            afterPhaseEvents.Dequeue().Do(this);
        else if (Next() != null)
            EndPhase(Next());
        else
            Game.EndGame();
    }

    public async virtual void StartPhase(Phase parent)
    {
        this.parent = parent;
        Game.currentPhase = this;

        Debug.Log($"Start Phase {name}");

        PhaseStartEvent?.Invoke(this); 
        phaseStartEvent?.Invoke();

        while (onPhaseEvents.Count > 0)
            await onPhaseEvents.Dequeue().Do(this); 

        Continue();
    }

    public virtual void EndPhase(Phase nextPhase)
    {
        PhaseEndEvent?.Invoke(this);
        phaseEndEvent?.Invoke(); 

        nextPhase.StartPhase(this); 
    }

    Phase Next()
    {
        List<Phase> children = GetComponentsInChildren<Phase>().ToList();        
        return children.Count >= 2 ? children[1] : transform.parent.GetComponent<Phase>()?.Next(this);
    }

    Phase Next(Phase child)
    {
        int i = child.transform.GetSiblingIndex() + 1;

        if (transform.childCount > i)
            return transform.GetChild(i).GetComponent<Phase>(); 
        else
            return transform.parent?.GetComponent<Phase>().Next(this); 
    }

    public T GetCurrent<T>() => GetComponentInParent<T>(); 
}