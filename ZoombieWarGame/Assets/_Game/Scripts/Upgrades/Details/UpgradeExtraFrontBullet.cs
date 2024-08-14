using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Survival
{
    [CreateAssetMenu(fileName = "UpgradeFrontBullet", menuName = "Upgrades/FrontBullet")]
    public class UpgradeExtraFrontBullet : Upgrade
    {
        public int ExtraFrontBulletIncresament;
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            AbilityEvents.OnUpgrade_ExtraFrontBullet?.Invoke(ExtraFrontBulletIncresament);
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

