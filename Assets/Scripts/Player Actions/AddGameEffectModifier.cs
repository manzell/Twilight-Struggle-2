using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class AddGameEffectModifier : PlayerAction
    {
        [SerializeField] Modifier mod;
        [SerializeField] Effect effect;

        public override Task Action()
        {
            if (mod != null)
                Game.modifiers.Add(mod);
            if (effect != null)
                Game.activeEffects.Add(effect);

            return Task.CompletedTask;
        }
    }
}