using System.Collections;
using System.Collections.Generic;
using HenryDev;
using Pooling;
using UnityEngine;

namespace Survival
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb2D;
        [SerializeField] GameObject vfxExplosion;
        private float damage;
        private int piercing = 0;
        public void Setup(Vector3 direction, float speed, float damage, int piercing)
        {
            this.damage = damage;
            this.piercing = piercing;

            direction = direction.IsNormalized() ? direction : direction.normalized;
            this.rb2D.velocity = direction * speed;
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Screen"))
            {
                Kill();
            }
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IChangeableValue health))
            {
                health.UpdateValue(-this.damage);
                Kill(true);
            }
        }
        void Kill(bool isExplosion = false)
        {
            if (isExplosion)
            {
                this.vfxExplosion.Spawn(transform.position, Quaternion.identity);
            }
            --this.piercing;
            if (this.piercing >= 0)
                return;
            gameObject.Despawn();
        }
    }
}
