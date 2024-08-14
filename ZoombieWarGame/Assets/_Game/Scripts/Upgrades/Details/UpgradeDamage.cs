using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Survival
{
    [CreateAssetMenu(fileName = "UpgradeDamage", menuName = "Upgrades/Damage")]
    public class UpgradeDamage : Upgrade
    {
        [Tooltip("In Percentage")]
        public float DamageIncreasament;
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            AbilityEvents.OnUpgrade_Damage?.Invoke(DamageIncreasament);
            if (this.isInfinite)
                return;
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

