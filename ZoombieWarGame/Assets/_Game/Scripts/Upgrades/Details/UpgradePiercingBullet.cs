using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    [CreateAssetMenu(fileName = "UpgradePiercingBullet", menuName = "Upgrades/UpgradePiercingBullet")]
    public class UpgradePiercingBullet : Upgrade
    {
        public int PiercingIncreasament;
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            AbilityEvents.OnUpgrade_Piercing?.Invoke(PiercingIncreasament);
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
