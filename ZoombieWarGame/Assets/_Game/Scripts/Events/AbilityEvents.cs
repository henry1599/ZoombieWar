using System;

namespace Survival
{
    public static class AbilityEvents
    {
        public static Action<float> OnUpgrade_RateOfFire;
        public static Action<float> OnUpgrade_Damage;
        public static Action<int> OnUpgrade_ExtraFrontBullet;
        public static Action<int> OnUpgrade_ExtraBackBullet;
        public static Action<float> OnUpgrade_Luck;
        public static Action<int> OnUpgrade_Piercing;
    }
}
