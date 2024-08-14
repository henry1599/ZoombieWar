using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Survival
{
    [CreateAssetMenu(fileName = "UpgradeBackBullet", menuName = "Upgrades/BackBullet")]
    public class UpgradeExtraBackBullet : Upgrade
    {
        public int ExtraBackBulletIncresament;
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            AbilityEvents.OnUpgrade_ExtraBackBullet?.Invoke(ExtraBackBulletIncresament);
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

