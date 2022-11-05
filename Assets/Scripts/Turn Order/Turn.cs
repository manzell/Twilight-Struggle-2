using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    public class Turn : Phase
    {
        public int turnNumber;
        public WarPhase warPhase;

        public HeadlinePhase headlinePhase => GetComponentInChildren<HeadlinePhase>();
        public Dictionary<Player, int> milOps;
    }
}