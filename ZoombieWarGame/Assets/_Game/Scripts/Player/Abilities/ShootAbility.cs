using System.Collections;
using System.Collections.Generic;
using HenryDev;
using NaughtyAttributes;
using Pooling;
using UnityEngine;

namespace Survival
{
    public class ShootAbility : Ability
    {
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] float bulletSpeed;
        [SerializeField] Transform shootPoint;
        [SerializeField] LayerMask ignoreLayer;
        [SerializeField] Color gizmosColor;




        private float timeBtwShots;
        private Vector2 shootingRange;
        private GameObject currentTrackedEnemy = null;


        private void Init()
        {
            this.shootingRange = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            this.timeBtwShots = PlayerStat.Instance.InverseRateOfFire;
        }
        public override void SetEnable(bool value)
        {
            base.SetEnable(value);
            Init();
        }
        public override void Run()
        {
            if (this.timeBtwShots > 0)
            {
                this.timeBtwShots -= Time.deltaTime;
                return;
            }
            if (this.currentTrackedEnemy == null)
                return;
            this.timeBtwShots = PlayerStat.Instance.InverseRateOfFire;
            var direction = this.currentTrackedEnemy.transform.position - shootPoint.position;
            direction = direction.IsNormalized() ? direction : direction.normalized;
            ShootFront(direction);
            ShootBack(direction);
        }

        public override void RunPhysics()
        {
            CastNearestEnemy(out this.currentTrackedEnemy);
        }
        void ShootFront(Vector2 direction)
        {
            Vector2 bulletPlacementDirection = Vector2.Perpendicular(direction);
            for (float i = -PlayerStat.Instance.FrontBullet / 2f; i <= PlayerStat.Instance.FrontBullet / 2f; i++)
            {
                Vector2 bulletDirection = direction + bulletPlacementDirection * i * PlayerStat.Instance.BulletGap;
                FireBullet(bulletDirection);
            }
        }
        void ShootBack(Vector2 direction)
        {
            Vector2 bulletPlacementDirection = Vector2.Perpendicular(direction);
            for (float i = -PlayerStat.Instance.BackBullet / 2f; i < PlayerStat.Instance.BackBullet / 2f; i++)
            {
                Vector2 bulletDirection = direction + bulletPlacementDirection * i * PlayerStat.Instance.BulletGap;
                FireBullet(-bulletDirection);
            }
        }
        void FireBullet(Vector2 direction)
        {
            GameObject bulletObject = bulletPrefab.Spawn(shootPoint.position, Quaternion.identity);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.Setup(direction, this.bulletSpeed, PlayerStat.Instance.Damage, PlayerStat.Instance.Piercing);
        }
        bool CastNearestEnemy(out GameObject enemyObject)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(shootPoint.position, this.shootingRange * 2f * 0.85f, 0, ~ignoreLayer);
            if (hits == null || hits.Length == 0)
            {
                enemyObject = null;
                return false;
            }

            float closestDistance = float.MaxValue;
            enemyObject = null;

            foreach (var hit in hits)
            {
                if (!hit.gameObject.activeSelf)
                    continue;

                var enemy = hit.gameObject.GetComponent<Enemy>();
                if (enemy == null)
                    continue;

                float distance = (shootPoint.position - hit.transform.position).sqrMagnitude;
                if (distance < closestDistance * closestDistance)
                {
                    closestDistance = distance;
                    enemyObject = enemy.gameObject;
                }
            }

            return enemyObject != null;
        }
        void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawWireCube(shootPoint.position, this.shootingRange * 2);
        }

        public void IncreaseDamage(float increasement)
        {
            PlayerStat.Instance.IncreaseDamage(increasement);
        }

        public void IncreaseExtraFrontBullet(int increasement)
        {
            PlayerStat.Instance.IncreaseExtraFrontBullet(increasement);
        }

        public void IncreaseExtraBackBullet(int increasement)
        {
            PlayerStat.Instance.IncreaseExtraBackBullet(increasement);
        }

        public void IncreaseRateOfFire(float increasement)
        {
            PlayerStat.Instance.IncreaseRateOfFire(increasement);
        }
        public void IncreasePiercing(int increasement)
        {
            PlayerStat.Instance.IncreasePiercing(increasement);
        }
    }
}
