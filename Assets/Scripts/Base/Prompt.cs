using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TwilightStruggle
{
    public class Prompt : MonoBehaviour
    {
        bool wait = true;

        public async Task Wait()
        {
            while (wait == true)
                await Task.Yield();
        }

        public void Confirm() => wait = false;
    }
}