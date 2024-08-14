using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pooling;
using HenryDev;

namespace Survival
{
    public class EnemySpawner : MonoBehaviour
    {
        public SpawnConfig spawnConfig;
        public Transform playerTransform;
        public Transform enemyContainer;
        public float spawnWidth = 10f;
        public float spawnHeight = 10f;
        public bool CanSpawn = true;
        private float inverseSpawnRate;
        private float spawnTimer;
        private SpawnData currentSpawnData;
        public SpawnData CurrentSpawnData => this.currentSpawnData; 
        void Start()
        {
            UpdateCurrentSpawnData();
            this.inverseSpawnRate = 1f / this.currentSpawnData.SpawnRate;
            this.spawnTimer = this.inverseSpawnRate;
        }
        void Update()
        {
            if (!CanSpawn)
                return;
            UpdateCurrentSpawnData();
            if (this.spawnTimer > 0)
            {
                this.spawnTimer -= Time.deltaTime;
                return;
            }
            this.spawnTimer = this.inverseSpawnRate;
            SpawnEnemy();
        }
        public void UpdateSpawnRate(float newRate)
        {
            if (this.currentSpawnData.SpawnRate == newRate)
                return;
            this.currentSpawnData.SpawnRate = newRate;
            this.inverseSpawnRate = 1f / this.currentSpawnData.SpawnRate;
        }

        private void SpawnEnemy()
        {
            Vector3 randomPosition = transform.RandomPositionOnEdgeRectangle(spawnWidth, spawnHeight);
            var enemyObject = GetEnemyPrefab()?.Spawn(randomPosition, Quaternion.identity, this.enemyContainer);
            var enemyInstance = enemyObject?.GetComponent<Enemy>();
            enemyInstance?.Setup(this.playerTransform);
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(this.spawnWidth, this.spawnHeight, 0));
        }
        void UpdateCurrentSpawnData()
        {
            if (this.currentSpawnData != null)
            {
                if (this.currentSpawnData.StartTime <= GameplayManager.Instance.GameTime && 
                    GameplayManager.Instance.GameTime <= this.currentSpawnData.EndTime)
                    return;
            }
            foreach (var spawnData in this.spawnConfig.SpawnDatas)
            {
                if (spawnData.StartTime <= GameplayManager.Instance.GameTime && GameplayManager.Instance.GameTime <= spawnData.EndTime)
                {
                    this.currentSpawnData = spawnData;
                    return;
                }
            }
        }
        GameObject GetEnemyPrefab()
        {
            if (this.currentSpawnData.SpawnEnemyData.Count == 0)
                return null;
            float total = this.currentSpawnData.TotalSpawnChance();
            float random = Random.Range(0, total);
            int count = this.currentSpawnData.SpawnEnemyData.Count;
            for (int i = 0; i < count; i++)
            {
                var spawnEnemyData = this.currentSpawnData.SpawnEnemyData[i];
                if (random <= spawnEnemyData.SpawnChance)
                {
                    return spawnEnemyData.Prefab;
                }
            }
            return null;
        }
    }
}
