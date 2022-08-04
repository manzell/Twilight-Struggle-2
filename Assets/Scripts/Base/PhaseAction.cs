using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; 

[System.Serializable]
public abstract class PhaseAction
{
    protected CountrySelectionManager selectionManager;
    public abstract Task Do(Phase phase); 
}
