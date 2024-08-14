using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public class CollectAbility : Ability
    {
        [SerializeField] float collectRadius;
        [SerializeField] LayerMask collectLayer;
        public override void Run()
        {
        }

        public override void RunPhysics()
        {
            CastCollectible();
        }

        void CastCollectible()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, collectRadius, collectLayer);
            foreach (var collider in colliders)
            {
                ICollectible collectible = collider.GetComponent<ICollectible>();
                if (collectible != null)
                {
                    collectible.Collect(gameObject);
                }
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, collectRadius);
        }
    }
}
