using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SelectionManager<T>
{
    public bool open = true;
    public Task<T> Selection => (selection = new TaskCompletionSource<T>()).Task;
    public IEnumerable<T> Selected => selected;
    public IEnumerable<T> Selectable => selectable;

    protected int selectionLimit = 0;
    protected List<T> selected = new(); 
    protected List<T> selectable = new();
    protected TaskCompletionSource<T> selection;
    protected System.Action<T> callback;

    public SelectionManager(IEnumerable<T> selection, int selectionLimit = 0)
    {
        this.selectionLimit = selectionLimit;

        foreach (ISelectable thing in selection)
            AddSelectable(thing); 
    }

    public SelectionManager(IEnumerable<T> selection, System.Action<T> callback)
    {
        this.callback = callback;

        foreach (ISelectable thing in selection)
            AddSelectable(thing);
    }

    public void AddSelectable(ISelectable thing)
    {
        selectable.Add((T)thing);
        thing.OnSelectable();
        thing.onClick += Select;
    }

    public void RemoveSelectable(ISelectable thing)
    {   
        selectable.Remove((T)thing);
        thing.onClick -= Select;
        thing.RemoveSelectable(); 
    }

    void Select(ISelectable thing)
    {
        if (selectionLimit > 0 && selected.Contains((T)thing))
            RemoveSelectable(thing);
        else if (selectionLimit == 0 || selected.Count < selectionLimit)
            selected.Add((T)thing);

        callback?.Invoke((T)thing);
        selection.TrySetResult((T)thing);
    }

    public void Close()
    {
        open = false;

        foreach (T thing in selectable.ToArray())
            RemoveSelectable((ISelectable)thing);
    }
}