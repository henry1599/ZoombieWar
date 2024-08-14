using System;
using System.Collections;
using System.Collections.Generic;
using HenryDev;
using UnityEngine;
using UnityEngine.UI;

namespace Survival
{
    public class UIExpBar : UIChangeableValueBar
    {
        void Awake()
        {
            PlayerEvents.OnExpInit += this.Init;
            PlayerEvents.OnExpGain += HandleExpGain;
            PlayerEvents.OnLevelUp += HandleLevelUp;
        }
        void OnDestroy()
        {
            PlayerEvents.OnExpGain -= HandleExpGain;
            PlayerEvents.OnLevelUp -= HandleLevelUp;
        }

        private void HandleExpGain(float expPercentage)
        {
            this.changeableValue.UpdateValue(expPercentage);
        }

        private void HandleLevelUp(int level, float maxExp)
        {
            this.changeableValue.UpdateMaxValue(maxExp);
            this.changeableValue.MakeEmpty();
        }
    }
}
