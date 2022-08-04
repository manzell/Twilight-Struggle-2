using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class Turn : Phase
{
    public int turnNumber;
    public WarPhase warPhase;

    public HeadlinePhase headlinePhase => GetComponentInChildren<HeadlinePhase>();

    public List<Space.SpaceAttempt> spaceAttempts = new(); 
}
