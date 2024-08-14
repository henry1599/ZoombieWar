using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    [CreateAssetMenu(fileName = "PlayerStatConfig", menuName = "Player/PlayerStatConfig")]
    [InlineEditor]
    public class PlayerStatConfig : ScriptableObject
    {
        public int BaseMaxHealth;
        public float BaseDamage;
        public float BaseRateOfFire;
        public float BulletGap;
        public int BaseFrontBullet;
        public int BaseBackBullet;
        [Range(0f, 1f)] public float Luck;
        public int Piercing;
        public float BasePickupRadius;
        public float BaseExp;
    }
}
