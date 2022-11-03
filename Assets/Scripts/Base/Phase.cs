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
    Phase previousPhase;
    public static event Action<Phase> PhaseStartEvent, PhaseEndEvent;

    public event Action phaseStartEvent, phaseEndEvent;

    //public Stack<PlayerAction> cardActions = new(); // These aren't card actions, these are really "Phase actions" 
    [field: SerializeField] public Queue<PhaseAction> onPhaseEvents { get; private set; } = new();
    [field: SerializeField] public Queue<PhaseAction> afterPhaseEvents { get; private set; } = new();

    //public void Push(PlayerAction cardAction) => cardActions.Push(cardAction); 

    public List<Modifier> modifiers { get; private set; } = new();
    public List<Effect> activeEffects { get; private set; } = new();
    [field:SerializeField] public List<PlayerAction> availableActions { get; private set; } 

    public async virtual void StartPhase(Phase previous)
    {
        previousPhase = previous;
        Game.currentPhase = this;

        Debug.Log($"Start Phase {name}");

        phaseStartEvent?.Invoke();
        PhaseStartEvent?.Invoke(this); 

        while (onPhaseEvents.Count > 0)
            await onPhaseEvents.Dequeue().Do(this);

        OnPhase(); 
    }

    public virtual void OnPhase() => EndPhase();

    public async virtual void EndPhase()
    {
        while (afterPhaseEvents.Count > 0)
            await afterPhaseEvents.Dequeue().Do(this);

        phaseEndEvent?.Invoke();
        PhaseEndEvent?.Invoke(this);

        if (Next() != null)
            Next().StartPhase(this);
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