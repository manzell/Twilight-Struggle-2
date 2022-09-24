using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Headline : TriggerCardEvent
{
    public override void SetPlayer(Player player)
    {
        name = $"{player.name} Headline";
        base.SetPlayer(player); 
    }
}
