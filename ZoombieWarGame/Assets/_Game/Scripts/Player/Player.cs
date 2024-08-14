using System.Collections;
using System.Collections.Generic;
using HenryDev;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public class Player : MonoBehaviour
    {
        private IChangeableValue health;
        private IChangeableValue exp;
        void Start()
        {
            this.health = gameObject.GetChangeableComponent<Health>();
            this.exp = gameObject.GetChangeableComponent<Exp>();
            
            this.health.InitValue(PlayerStat.Instance.MaxHealth, startFrom0: false);
            this.exp.InitValue(PlayerStat.Instance.BaseExp, startFrom0: true);

            PlayerEvents.OnHealthInit?.Invoke(this.health);
            PlayerEvents.OnExpInit?.Invoke(this.exp);
        }

        [Button("Heal")]
        public void Heal()
        {
            this.health.UpdateValue(10);
        }
        [Button("Take damage")]
        public void TakeDamage()
        {
            this.health.UpdateValue(-10);
        }
    }
}
