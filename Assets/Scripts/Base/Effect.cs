using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TwilightStruggle
{
    public abstract class Effect
    {
        public abstract bool Test(PlayerAction t);

        public virtual void Apply() { }
    }
}