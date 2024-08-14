using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.UI;

namespace Survival
{
    public class UIUpgradeItem : MonoBehaviour
    {
        [SerializeField] TMP_Text title;
        [SerializeField] Image icon;
        [SerializeField] TMP_Text description;
        [SerializeField] Button button;
        [SerializeField] Image mainPanel;
        [SerializeField] Image rarityPanel;
        [SerializeField] Image titlePanel;
        [SerializeField] TMP_Text rarityTitle;
        [SerializeField] UpgradeUIRarityDataDict rarityData;
        Upgrade upgrade;
        Action extraClickedCallback = null;
        public void Setup(Upgrade upgrade, Action onClicked = null)
        {
            this.upgrade = upgrade;
            this.title.text = upgrade.UpgradeName;
            this.icon.sprite = upgrade.Icon;
            this.description.text = upgrade.Description;
            this.extraClickedCallback = onClicked;

            this.mainPanel.color = this.rarityData[upgrade.Rarity].MainPanelColor;
            this.rarityPanel.color = this.rarityData[upgrade.Rarity].RarityTitleColor;
            this.titlePanel.color = this.rarityData[upgrade.Rarity].PanelTitleColor;
            this.rarityTitle.color = this.rarityData[upgrade.Rarity].TextColor;
            this.rarityTitle.text = upgrade.Rarity.ToString();

            this.button.onClick.AddListener(OnClicked);
        }
        void OnDestroy()
        {
            this.button.onClick.RemoveListener(OnClicked);
        }
        void OnClicked()
        {
            UpgradeManager.Instance.Upgrade(this.upgrade);
            this.extraClickedCallback?.Invoke();
        }

        [Serializable]
        public class UpgradeUIRarityDataDict : SerializableDictionaryBase<eRarity, UpgradeUIRarityData> { }
        [Serializable]
        public class UpgradeUIRarityData
        {
            public Color MainPanelColor;
            public Color PanelTitleColor;
            public Color RarityTitleColor;
            public Color TextColor;
        }
    }
}
