using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HenryDev;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Survival
{
    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance {get; private set;}
        [ReadOnly, SerializeField] UpgradeDict upgrades;
        public UpgradeDict Upgrades => this.upgrades;
        // AnimationCurve fields for rarity chances
        [SerializeField] AnimationCurve commonChanceCurve;
        [SerializeField] AnimationCurve rareChanceCurve;
        [SerializeField] AnimationCurve epicChanceCurve;
        [SerializeField] AnimationCurve legendaryChanceCurve;
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        public void Upgrade(Upgrade upgrade)
        {
            upgrade.ApplyUpgrade();
        }
        [Button("Gather Upgrades data")]
        public void GatherUpgrades()
        {
            var rarities = Enum.GetValues(typeof(eRarity)).Cast<eRarity>().ToList();
            foreach (var rarity in rarities)
            {
                upgrades[rarity] = new UpgradeList
                {
                    Rarity = rarity,
                    Upgrades = Resources.LoadAll<Upgrade>(string.Format("Upgrades/{0}", rarity.ToString())).ToList()
                };
            }
        }
        // Get at most 3 upgrades using player luck to random rarity of upgrades
        public List<Upgrade> GetAvailableUpgrades()
        {
            List<Upgrade> availableUpgrades = new List<Upgrade>();
            List<Upgrade> allUpgrades = upgrades.Values.SelectMany(upgradeList => upgradeList.Upgrades).ToList();

            allUpgrades = allUpgrades.Shuffle();

            // Get probabilities from AnimationCurves
            float luck = PlayerStat.Instance.Luck;
            float commonChance = commonChanceCurve.Evaluate(luck);
            float rareChance = rareChanceCurve.Evaluate(luck);
            float epicChance = epicChanceCurve.Evaluate(luck);
            float legendaryChance = legendaryChanceCurve.Evaluate(luck);

            // Normalize probabilities
            float totalChance = commonChance + rareChance + epicChance + legendaryChance;
            commonChance /= totalChance;
            rareChance /= totalChance;
            epicChance /= totalChance;
            legendaryChance /= totalChance;

            // Filter upgrades based on calculated probabilities
            List<Upgrade> filteredUpgrades = allUpgrades.Where(upgrade =>
            {
                float randomValue = UnityEngine.Random.value;
                switch (upgrade.Rarity)
                {
                    case eRarity.Common:
                        return randomValue <= commonChance;
                    case eRarity.Rare:
                        return randomValue <= rareChance;
                    case eRarity.Epic:
                        return randomValue <= epicChance;
                    case eRarity.Legendary:
                        return randomValue <= legendaryChance;
                    default:
                        return false;
                }
            }).ToList();

            // Get at most 3 upgrades based on filtered list
            int maxUpgrades = 3;
            int numUpgrades = Mathf.Min(maxUpgrades, filteredUpgrades.Count);
            for (int i = 0; i < numUpgrades; i++)
            {
                Upgrade upgrade = filteredUpgrades[i];
                availableUpgrades.Add(upgrade);
            }

            return availableUpgrades;
        }
        

        [Serializable]
        public class UpgradeDict : SerializableDictionaryBase<eRarity, UpgradeList> { }
        [Serializable]
        public class UpgradeList
        {
            public eRarity Rarity;
            public List<Upgrade> Upgrades;
        }
    }
}
