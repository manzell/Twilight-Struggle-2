using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    [System.Serializable]
    public abstract class PhaseAction
    {
        public abstract Task Do(Phase phase);
    }
}