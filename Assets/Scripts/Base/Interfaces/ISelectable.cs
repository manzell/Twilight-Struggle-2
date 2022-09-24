using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public event Action<ISelectable> selectionEvent;
    public abstract void Select();
}