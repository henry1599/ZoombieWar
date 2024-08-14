using System;
using System.Collections;
using System.Collections.Generic;
using HenryDev.Utilities;
using NaughtyAttributes;
using UnityEngine;

namespace Survival
{
    public class UIUpgradePanel : MonoBehaviour
    {
        [SerializeField] UIUpgradeItem item;
        [SerializeField] Transform container;
        [SerializeField] GameObject background;
        void Awake()
        {
            PlayerEvents.OnLevelUp += HandleLevelUp;
        }
        void OnDestroy()
        {
            PlayerEvents.OnLevelUp -= HandleLevelUp;
        }

        private void HandleLevelUp(int level, float maxExp)
        {
            OpenPanel();
            this.container.DeleteChildren();
            var availableUpgrades = UpgradeManager.Instance.GetAvailableUpgrades();
            if (availableUpgrades.Count == 0)
            {
                return;
            }
            foreach (var upgrade in availableUpgrades)
            {
                var upgradeItem = Instantiate(item, this.container);
                upgradeItem.Setup(upgrade, ClosePanel);
            }
        }
        [Button]
        void OpenPanel()
        {
            Time.timeScale = 0;
            this.background.SetActive(true);
        }
        [Button]
        void ClosePanel()
        {
            Time.timeScale = 1;
            this.container.DeleteChildren();
            this.background.SetActive(false);
        }
    }
}
