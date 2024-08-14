using System.Collections;
using System.Collections.Generic;
using HenryDev;
using UnityEngine;

namespace Survival
{
    public class Enemy_Melee : MonoBehaviour
    {
        [SerializeField] float damage = 5f;
        [SerializeField] LayerMask attackableLayer;
        void OnCollisionEnter2D(Collision2D other)
        {
            IChangeableValue health = null;
            bool isMatchLayer = other.gameObject.IsMatchLayer(this.attackableLayer);
            bool hasHealthComp = other.gameObject.TryGetComponent(out health);
            bool canDealDamage = isMatchLayer && hasHealthComp;
            if (canDealDamage)
            {
                this.Log(string.Format("Take damage {0}", this.damage));
                health?.UpdateValue(-this.damage);
            }    
        }
    }
}
