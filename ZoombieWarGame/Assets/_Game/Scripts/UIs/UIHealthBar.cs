using System.Collections;
using System.Collections.Generic;
using HenryDev;
using UnityEngine;

namespace Survival
{
    public class UIHealthBar : UIChangeableValueBar
    {
        void Awake()
        {
            PlayerEvents.OnHealthInit += this.Init;
        }
    }
}
