using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public abstract class Effect 
{
    public abstract bool Test(PlayerAction t); 

    public virtual void Apply() { }
}