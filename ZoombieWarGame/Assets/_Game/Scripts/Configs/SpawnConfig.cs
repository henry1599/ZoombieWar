using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Survival
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "Configs/SpawnConfig")]
    public class SpawnConfig : ScriptableObject
    {
        public List<SpawnData> SpawnDatas;
    }
    [Serializable]
    public class SpawnData
    {
        public float StartTime;
        public float EndTime;
        [AllowNesting]
        public List<SpawnEnemyData> SpawnEnemyData;
        public float SpawnRate;
        public float TotalSpawnChance()
        {
            float result = 0;
            foreach (var spawnEnemyData in SpawnEnemyData)
            {
                result += spawnEnemyData.SpawnChance;
            }
            return result;
        }
        [Button]
        public void ValidateData()
        {
            SpawnEnemyData.Sort((a, b) => a.SpawnChance.CompareTo(b.SpawnChance));
        }
    }
    [Serializable]
    public class SpawnEnemyData
    {
        public float SpawnChance;
        public GameObject Prefab;
    }
}
