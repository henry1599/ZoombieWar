using System;
using System.Collections;
using System.Collections.Generic;
using HenryDev;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Collider2D col2D;
        private IChangeableValue health;
        private IChangeableValue exp;
        void Awake()
        {
            PlayerEvents.OnMapGenerated += HandleMapGenerated;
        }
        void Start()
        {
            this.health = gameObject.GetChangeableComponent<Health>();
            this.exp = gameObject.GetChangeableComponent<Exp>();
            
            this.health.InitValue(PlayerStat.Instance.MaxHealth, startFrom0: false);
            this.exp.InitValue(PlayerStat.Instance.BaseExp, startFrom0: true);

            PlayerEvents.OnHealthInit?.Invoke(this.health);
            PlayerEvents.OnExpInit?.Invoke(this.exp);


        }
        void OnDestroy()
        {
            PlayerEvents.OnMapGenerated -= HandleMapGenerated;
        }

        private void HandleMapGenerated()
        {
            this.col2D.isTrigger = true;
            Cell spawnCell = MapManager.Instance?.GetSpawnCell();
            if (spawnCell == null)
            {
                this.Log("Spawn cell is null", Color.red);
                return;
            }
            this.transform.position = spawnCell.Position;
            
            this.col2D.isTrigger = false;
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
