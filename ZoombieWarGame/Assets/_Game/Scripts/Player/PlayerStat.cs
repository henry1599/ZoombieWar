using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public class PlayerStat : MonoBehaviour
    {
        public static PlayerStat Instance {get; private set;}
        public PlayerStatConfig StatConfig;
        [ShowInInspector, ReadOnly] public float Damage { get; private set; }
        [ShowInInspector, ReadOnly] public float RateOfFire { get; private set; }
        [ShowInInspector, ReadOnly] public float BulletGap { get; private set; }
        [ShowInInspector, ReadOnly] public int FrontBullet { get; private set; }
        [ShowInInspector, ReadOnly] public int BackBullet { get; private set; }
        [ShowInInspector, ReadOnly] public float Luck { get; private set; }
        [ShowInInspector, ReadOnly] public float InverseRateOfFire { get; private set;} 
        [ShowInInspector, ReadOnly] public int Piercing { get; private set; }
        [ShowInInspector, ReadOnly] public int MaxHealth { get; private set; }
        [ShowInInspector, ReadOnly] public float PickupRadius { get; private set; }
        [ShowInInspector, ReadOnly] public float BaseExp { get; private set; }
        [ShowInInspector, ReadOnly] public float RateOfMine { get; private set; }
        [ShowInInspector, ReadOnly] public float InverseRateOfMining { get; private set; }



        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            Damage = this.StatConfig.BaseDamage;
            RateOfFire = this.StatConfig.BaseRateOfFire;
            BulletGap = this.StatConfig.BulletGap;
            FrontBullet = this.StatConfig.BaseFrontBullet;
            BackBullet = this.StatConfig.BaseBackBullet;
            Luck = this.StatConfig.Luck;
            InverseRateOfFire = 1f / RateOfFire;
            Piercing = this.StatConfig.Piercing;
            MaxHealth = this.StatConfig.BaseMaxHealth;
            PickupRadius = this.StatConfig.BasePickupRadius;
            BaseExp = this.StatConfig.BaseExp;
            RateOfMine = this.StatConfig.RateOfMine;
            InverseRateOfMining = 1f / this.StatConfig.RateOfMine;
        }
        public void IncreateRateOfMining(float percentage)
        {
            RateOfMine += RateOfMine * percentage;
            InverseRateOfMining = 1f / RateOfMine;
        }
        public void IncreasePickupRadius(int value)
        {
            PickupRadius += value;
        }
        public void IncreaseMaxHealth(int value)
        {
            MaxHealth += value;
        }
        public void IncreaseDamage(float percentage)
        {
            Damage += Damage * percentage;
        }
        public void IncreaseRateOfFire(float percentage)
        {
            RateOfFire += RateOfFire * percentage;
            InverseRateOfFire = 1f / RateOfFire;
        }
        public void IncreaseExtraFrontBullet(int value)
        {
            FrontBullet += value;
        }       
        public void IncreaseExtraBackBullet(int value)
        {
            BackBullet += value;
        }
        public void IncreaseLuck(float percentage)
        {
            Luck += Luck * percentage;
        }
        public void IncreasePiercing(int value)
        {
            Piercing += value;
        }
    }
}
