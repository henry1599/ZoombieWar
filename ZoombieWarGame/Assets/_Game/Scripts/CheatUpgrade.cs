using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Survival.Cheat
{
    public class CheatUpgrade : MonoBehaviour
    {
        [SerializeField] string rateOfFireUpgradeId;
        [SerializeField] string damageUpgradeId;
        [SerializeField] string extraFrontBulletUpgradeId;
        [SerializeField] string extraBackBulletUpgradeId;
        // [Button("Upgrade Rate of Fire")]
        // public void UpgradeRateOfFire()
        // {
        //     UpgradeManager.Instance.Upgrade(rateOfFireUpgradeId);
        // }
        // [Button("Upgrade Damage")]
        // public void UpgradeDamage()
        // {
        //     UpgradeManager.Instance.Upgrade(damageUpgradeId);
        // }
        // [Button("Upgrade Extra Front Bullet")]
        // public void UpgradeExtraFrontBullet()
        // {
        //     UpgradeManager.Instance.Upgrade(extraFrontBulletUpgradeId);
        // }
        // [Button("Upgrade Extra Back Bullet")]
        // public void UpdateExtraBackBullet()
        // {
        //     UpgradeManager.Instance.Upgrade(extraBackBulletUpgradeId);
        // }
    }
}
