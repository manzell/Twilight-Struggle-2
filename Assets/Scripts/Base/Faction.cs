using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI; 

public class Faction : ScriptableObject
{
    public Color controlColor, accentColor;
    public Faction enemyFaction; 
    public Sprite factionIcon, flag;
    public Player player; 
}
