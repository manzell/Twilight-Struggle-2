using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TwilightStruggle
{
    public class CancelHeadline : PlayerAction
    {
        public override Task Action() => Task.CompletedTask;
    }
}