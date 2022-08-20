using UnityEngine; 
using System;
using System.Linq; 
using System.Collections.Generic;
using System.Threading.Tasks;

public class SelectionManager<T>
{
    public static event Action<SelectionManager<T>> SelectionStartEvent, SelectionEndEvent;
    public bool open { get; private set; } = true;
    public Task<T> Selection => (selectionTask = new()).Task;
    public IEnumerable<T> Selected => selected.Cast<T>();
    public IEnumerable<T> Selectables => selectables.Cast<T>(); 
    public IEnumerable<T> InitialSelection => initialSelection.Cast<T>(); 

    protected int selectionLimit = 0;
    protected List<ISelectable> selected = new(); 
    protected List<ISelectable> selectables = new();
    protected List<ISelectable> initialSelection = new();
    protected TaskCompletionSource<T> selectionTask = new();
    protected Action<T> callback;

    public event Action<ISelectable> selectEvent, deselectEvent, addSelectableEvent, removeSelectableEvent; 

    public SelectionManager(IEnumerable<ISelectable> selection, int selectionLimit = 0)
    {
        this.initialSelection = selection.ToList();
        this.selectionLimit = selectionLimit;

        SelectionStartEvent?.Invoke(this); 

        foreach (ISelectable thing in selection)
            AddSelectable(thing); 
    }

    public SelectionManager(IEnumerable<ISelectable> selection, Action<T> callback)
    {
        this.initialSelection = selection.ToList();
        this.callback = callback;

        SelectionStartEvent?.Invoke(this);

        foreach (ISelectable thing in selection)
            AddSelectable(thing);
    }

    public void AddSelectable(ISelectable thing)
    {
        selectables.Add(thing);
        addSelectableEvent?.Invoke(thing);
        thing.selectionEvent += Select; 
    }

    public void RemoveSelectable(ISelectable thing)
    {
        selectables.Remove(thing);
        removeSelectableEvent?.Invoke(thing);
        thing.selectionEvent -= Select;
    }

    public void RemoveSelectables(IEnumerable<ISelectable> things)
    {
        foreach (ISelectable thing in things.ToArray())
            RemoveSelectable(thing); 
    }

    void Select(ISelectable thing)
    {
        if (selectionLimit > 0 && selected.Contains(thing))
        {
            selected.Remove(thing);
            deselectEvent?.Invoke(thing);
        }
        else if (selectionLimit == 0 || selected.Count < selectionLimit)
        {
            selected.Add(thing);
            selectEvent?.Invoke(thing);
            callback?.Invoke((T)thing);
        }

        // This happened last time when we were calling it twice...
        // Because we ARE calling it twice.

        selectionTask.TrySetResult((T)thing); // Maybe this shouldn't be where we set the Result. Maybe whereever we start the selection event instead?
    }

    public void Close()
    {
        open = false;
        RemoveSelectables(selectables);
        SelectionEndEvent?.Invoke(this);
    }
}