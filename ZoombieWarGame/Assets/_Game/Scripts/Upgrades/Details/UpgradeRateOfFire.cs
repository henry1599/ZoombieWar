using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace Survival
{
    [CreateAssetMenu(fileName = "UpgradeRateOfFire", menuName = "Upgrades/RateOfFire")]
    public class UpgradeRateOfFire : Upgrade
    {
        [Tooltip("In Percentage")]
        public float RateOfFireIncreasament;
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            AbilityEvents.OnUpgrade_RateOfFire?.Invoke(RateOfFireIncreasament);
            --this.quantity;
        }
        public override bool CanUpgrade()
        {
            if (this.isInfinite)
                return true;
            return this.quantity > 0;
        }
    }
}
