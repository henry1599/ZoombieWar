using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public class ExpManager : MonoBehaviour
    {
        public static ExpManager Instance { get; private set; }
        [ReadOnly, SerializeField] float currentExp = 0;
        [ReadOnly, SerializeField] float lifeTimeExp = 0;
        [ReadOnly, SerializeField] int currentLevel = 1;
        [ReadOnly, SerializeField] float currentMaxExp;
        [SerializeField] float maxExpMultiplier = 1.5f;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            PlayerEvents.OnExpCollected += HandleExpCollected;
        }
        private void OnDestroy()
        {
            PlayerEvents.OnExpCollected -= HandleExpCollected;
        }
        void Start()
        {
            this.currentMaxExp = PlayerStat.Instance.BaseExp;
        }
        void HandleExpCollected(float exp)
        {
            AddExp(exp);
        }
        public void AddExp(float exp)
        {
            this.currentExp += exp;
            this.lifeTimeExp += exp;
            PlayerEvents.OnExpGain?.Invoke(exp);
            while (this.currentExp >= this.currentMaxExp)
            {
                var extra = this.currentExp - this.currentMaxExp;
                this.currentExp = extra;
                this.currentLevel++;
                this.currentMaxExp = this.currentMaxExp * this.maxExpMultiplier;
                PlayerEvents.OnLevelUp?.Invoke(this.currentLevel, this.currentMaxExp);
                PlayerEvents.OnExpGain?.Invoke(extra);
            }
        }
    }
}
