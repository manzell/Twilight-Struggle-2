using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class PreventEffect : PlayerAction
    {
        [SerializeField] Effect effect;

        public override Task Action()
        {
            return Task.CompletedTask;
        }
    }
}