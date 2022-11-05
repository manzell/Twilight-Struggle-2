using UnityEngine; 
using System;
using System.Linq; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class SelectionManager<T> where T : ISelectable
    {
        public static event Action<SelectionManager<T>> SelectionStartEvent, SelectionEndEvent;

        public bool open { get; private set; } = true;
        public IEnumerable<ISelectable> Selected => selected;
        public IEnumerable<ISelectable> Selectables => selectables;
        public IEnumerable<ISelectable> InitialSelection => initialSelection;

        protected int selectionLimit = 0;
        protected List<ISelectable> selected = new();
        protected List<ISelectable> selectables = new();
        protected List<ISelectable> initialSelection = new();

        public TaskCompletionSource<ISelectable> selectionTaskSource = new();
        public Task<ISelectable> Selection => selectionTaskSource.Task;

        Action<ISelectable> callback;

        public event Action<ISelectable> selectEvent, deselectEvent, addSelectableEvent, removeSelectableEvent;

        public Action<ISelectable> AddSelectableEvent() => addSelectableEvent;

        public SelectionManager(IEnumerable<ISelectable> selection, int selectionLimit = 0)
        {
            this.selectionLimit = selectionLimit;
            initialSelection = selection.ToList();

            foreach (ISelectable thing in selection)
                AddSelectable(thing);

            SelectionStartEvent?.Invoke(this);
        }

        public SelectionManager(IEnumerable<ISelectable> selection, Action<ISelectable> callback)
        {
            initialSelection = selection.ToList();
            this.callback = callback;

            SelectionStartEvent?.Invoke(this);

            foreach (ISelectable thing in selection)
                AddSelectable(thing);
        }

        public SelectionManager(IEnumerable<ISelectable> selection)
        {
            initialSelection = selection.ToList();
            SelectionStartEvent?.Invoke(this);

            foreach (ISelectable thing in selection)
                AddSelectable(thing);
        }

        public void AddSelectable(ISelectable thing)
        {
            selectables.Add(thing);
            addSelectableEvent?.Invoke(thing);
            callback?.Invoke(thing);

            thing.selectionEvent += OnSelect;
        }

        public void RemoveSelectable(ISelectable thing)
        {
            selectables.Remove(thing);
            removeSelectableEvent?.Invoke(thing);
            thing.selectionEvent -= OnSelect;
        }

        public void RemoveSelectables(IEnumerable<ISelectable> things)
        {
            foreach (ISelectable thing in things.ToArray())
                RemoveSelectable(thing);
        }

        void OnSelect(ISelectable thing)
        {
            if (selected.Contains(thing) && selectionLimit > 0)
            {
                selected.Remove(thing);
                deselectEvent?.Invoke(thing);
            }
            else if(selectionLimit == 0 || selected.Count() < selectionLimit)
            {
                selected.Add(thing);
                selectEvent?.Invoke(thing);
                selectionTaskSource.SetResult(thing);
            }
        }

        public void Close()
        {
            open = false;
            RemoveSelectables(selectables);
            SelectionEndEvent?.Invoke(this);
        }
    }
}