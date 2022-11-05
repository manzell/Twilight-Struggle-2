using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class AdjustDEFCON : PlayerAction
    {
        [SerializeField] int DEFCONAdjustment;

        public override Task Action()
        {
            Game.AdjustDEFCON(DEFCONAdjustment);
            return Task.CompletedTask;
        }
    }
}