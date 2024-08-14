using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    [CreateAssetMenu(fileName = "UpgradeLuck", menuName = "Upgrades/UpgradeLuck")]
    public class UpgradeLuck : Upgrade
    {
        [Tooltip("In Percentage")]
        public float LuckIncreasament;
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            AbilityEvents.OnUpgrade_Luck?.Invoke(LuckIncreasament);
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
