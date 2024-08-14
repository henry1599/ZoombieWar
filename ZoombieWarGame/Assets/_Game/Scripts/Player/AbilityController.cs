using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public class AbilityController : MonoBehaviour
    {
        private List<Ability> abilities = new List<Ability>();
        void Awake()
        {
            GatherAbilities();


            AbilityEvents.OnUpgrade_RateOfFire += HandleUpgradeRateOfFire;
            AbilityEvents.OnUpgrade_Damage += HandleUpgradeDamage;
            AbilityEvents.OnUpgrade_ExtraFrontBullet += HandleUpgradeExtraFrontBullet;
            AbilityEvents.OnUpgrade_ExtraBackBullet += HandleUpgradeExtraBackBullet;
            AbilityEvents.OnUpgrade_Piercing += HandleUpgradePiercing;
            AbilityEvents.OnUpgrade_Luck += HandleUpgradeLuck;
        }
        void OnDestroy()
        {
            AbilityEvents.OnUpgrade_RateOfFire -= HandleUpgradeRateOfFire;
            AbilityEvents.OnUpgrade_Damage -= HandleUpgradeDamage;
            AbilityEvents.OnUpgrade_ExtraFrontBullet -= HandleUpgradeExtraFrontBullet;
            AbilityEvents.OnUpgrade_ExtraBackBullet -= HandleUpgradeExtraBackBullet;
            AbilityEvents.OnUpgrade_Piercing -= HandleUpgradePiercing;
            AbilityEvents.OnUpgrade_Luck -= HandleUpgradeLuck;
        }

        private void HandleUpgradeLuck(float increasement)
        {
            PlayerStat.Instance.IncreaseLuck(increasement);
        }

        private void HandleUpgradePiercing(int increasement)
        {
            foreach (var ability in abilities)
            {
                if (ability is ShootAbility shootAbility)
                {
                    shootAbility.IncreasePiercing(increasement);
                }
            }
        }

        private void HandleUpgradeDamage(float increasement)
        {
            foreach (var ability in abilities)
            {
                if (ability is ShootAbility shootAbility)
                {
                    shootAbility.IncreaseDamage(increasement);
                }
            }
        }

        private void HandleUpgradeExtraFrontBullet(int increasement)
        {
            foreach (var ability in abilities)
            {
                if (ability is ShootAbility shootAbility)
                {
                    shootAbility.IncreaseExtraFrontBullet(increasement);
                }
            }
        }

        private void HandleUpgradeExtraBackBullet(int increasement)
        {
            foreach (var ability in abilities)
            {
                if (ability is ShootAbility shootAbility)
                {
                    shootAbility.IncreaseExtraBackBullet(increasement);
                }
            }
        }

        private void HandleUpgradeRateOfFire(float increasement)
        {
            foreach (var ability in abilities)
            {
                if (ability is ShootAbility shootAbility)
                {
                    shootAbility.IncreaseRateOfFire(increasement);
                }
            }
        }

        void Start()
        {
            if (abilities != null)
            {
                foreach (var ability in abilities)
                {
                    ability.SetEnable(true);
                }
            }
        }
        void GatherAbilities()
        {
            var comps = GetComponents<Ability>();
            foreach (var comp in comps)
            {
                if (comp == null)
                    continue;
                if (this.abilities.Contains(comp))
                    continue;
                this.abilities.Add(comp);
            }
        }
    }
}
