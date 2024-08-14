using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Pooling;
using UnityEngine;
using DG.Tweening;

namespace Survival
{
    public class Item : MonoBehaviour, ICollectible
    {
        [SerializeField] float collectRadius = 0.25f;
        [SerializeField] float knockbackForce = 5;
        [SerializeField] float knockbackDuration = 0.5f;
        [SerializeField] protected bool vfxCollectedEnable = false;
        [ShowIf("vfxCollectedEnable"), SerializeField] protected eResourceId vfxCollectedPrefabId;
        private Transform target;
        private float knockbackTimer;
        private bool isDetected = false;
        void OnEnable()
        {
            this.isDetected = false;
            this.knockbackTimer = this.knockbackDuration;
        }
        void Update()
        {
            if (!this.isDetected)
                return;
            if (this.target == null)
                return;
            if (this.knockbackTimer > 0)
            {
                this.knockbackTimer -= Time.deltaTime;
                KnockBack();
                return;
            }
            MoveToTarget();
        }
        public void Collect(GameObject target)
        {
            transform.DOKill();
            if (this.isDetected)
                return;
            this.isDetected = true;
            this.target = target.transform;
        }
        void MoveToTarget()
        {
            if (this.target == null)
            {
                return;
            }
            Vector3 direction = (this.target.position - this.transform.position).normalized;
            this.transform.position += direction * 5 * Time.deltaTime;
            if ((this.transform.position - this.target.position).sqrMagnitude < this.collectRadius * this.collectRadius)
            {
                OnCollect();
            }
        }
        void KnockBack()
        {
            if (this.target == null)
            {
                return;
            }
            Vector3 direction = (this.target.position - this.transform.position).normalized;
            this.transform.position += -direction * this.knockbackForce * Time.deltaTime;
        }
        protected virtual void OnCollect()
        {
            if (this.vfxCollectedEnable)
            {
                var prefab = ResourceManager.Instance.GetResourceData(this.vfxCollectedPrefabId)?.gameObject;
                prefab.Spawn(this.transform.position);
            }

            gameObject.Despawn();
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, this.collectRadius);
        }
    }
}
