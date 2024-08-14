using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public class MineAbility : Ability
    {
        [SerializeField] float miningRadius;
        private float timeBtwMining;
        private bool canMine = false;
        public override void Run()
        {
            if (this.timeBtwMining > 0)
            {
                this.timeBtwMining -= Time.deltaTime;
                return;
            }
            this.timeBtwMining = PlayerStat.Instance.InverseRateOfMining;
            this.canMine = true;
            Mine();
        }
        public override void SetEnable(bool value)
        {
            base.SetEnable(value);
            Init();
        }
        public override void RunPhysics()
        {
        }
        void Mine()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, miningRadius);
            foreach (var collider in colliders)
            {
                var breakable = collider.GetComponent<IBreakable>();
                if (breakable != null)
                {
                    breakable.Mine(1);
                }
            }
        }
        void Init()
        {
            this.timeBtwMining = PlayerStat.Instance.InverseRateOfMining;
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, miningRadius);
        }
    }
}
