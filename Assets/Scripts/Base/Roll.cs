using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll 
{
    public int Value => roll + modifier;
    int roll, modifier; 

    public Roll (int mod)
    {
        roll = Random.Range(0, 6) + 1;
        modifier = mod; 
    }
}
