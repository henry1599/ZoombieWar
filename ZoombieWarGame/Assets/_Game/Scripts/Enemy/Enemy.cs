using System.Collections;
using System.Collections.Generic;
using HenryDev;
using Pooling;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Survival
{
    [System.Serializable]
    public class ExpDropDict : SerializableDictionaryBase<eExpType, float> { }
    public class Enemy : MonoBehaviour, IDroppable
    {
        [SerializeField] protected float speed = 1f;
        [SerializeField] protected float explosionRadius = 1f;
        [SerializeField] protected Transform graphic;
        [SerializeField] protected float maxHealth = 10;
        [SerializeField] protected ExpDropDict expDrops;
        [Range(0, 5), SerializeField] protected float dropRadius = 0.2f;
        [MinMaxSlider(1, 20), SerializeField] protected Vector2Int dropAmount;
        protected Transform playerTransform;
        protected Transform thisTransform;
        protected IChangeableValue health;

        void Awake()
        {
            this.thisTransform = transform;
            this.health = gameObject.GetChangeableComponent<Health>();
        }

        public void Setup(Transform playerTransform)
        {
            this.playerTransform = playerTransform;
            this.health.InitValue(this.maxHealth, startFrom0: false);

            this.health.OnEmpty += Die;
        }
        void OnDisable()
        {
            this.playerTransform = null;
            this.health.OnEmpty -= Die;
        }
        void Update()
        {
            MoveToPlayer();
        }
        void MoveToPlayer()
        {
            if (this.playerTransform == null)
            {
                return;
            }
            Vector3 direction = (this.playerTransform.position - this.transform.position).normalized;
            this.thisTransform.position += direction * this.speed * Time.deltaTime;
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.graphic.position, this.explosionRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.graphic.position, this.dropRadius);
        }
        public void Die()
        {
            Drop(this.dropAmount.RandomInt());
            gameObject.Despawn();
        }

        public void Drop(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var dropObject = GetDropItem();
                var pos = transform.RandomPositionInsideCircle(this.dropRadius);
                var obj = dropObject?.Spawn(transform.position);
                obj?.transform.DOMove(pos, 0.35f).SetEase(Ease.InOutSine);
            }
        }
        GameObject GetDropItem()
        {
            GameObject expItem = null;
            eExpType expType = GetExpTypeByChance();
            switch (expType)
            {
                case eExpType.Small:
                    expItem = ResourceManager.Instance.GetResourceData(eResourceId.SmallExp)?.gameObject;
                    break;
                case eExpType.Medium:
                    expItem = ResourceManager.Instance.GetResourceData(eResourceId.MediumExp)?.gameObject;
                    break;
                case eExpType.Large:
                    expItem = ResourceManager.Instance.GetResourceData(eResourceId.LargeExp)?.gameObject;
                    break;
            }
            return expItem;
        }
        eExpType GetExpTypeByChance()
        {
            float total = 0;
            foreach (var item in this.expDrops)
            {
                total += item.Value;
            }
            float random = Random.Range(0, total);
            foreach (var item in this.expDrops)
            {
                if (random < item.Value)
                {
                    return item.Key;
                }
            }
            return eExpType.Small;
        }
    }
}
