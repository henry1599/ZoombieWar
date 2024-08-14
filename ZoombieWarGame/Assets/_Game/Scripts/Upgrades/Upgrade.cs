using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Survival
{
    public enum eRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
    public class Upgrade : ScriptableObject
    {
        public eRarity Rarity;
        [SerializeField] protected bool isInfinite = false;
        [SerializeField, HideIf("isInfinite")] protected int quantity = 1;
        [ShowNativeProperty] public string Id 
        {
            get
            {
                if (string.IsNullOrEmpty(this.id))
                {
                    RebuildId();
                }
                return this.id;
            }
        } string id = string.Empty;
        public string UpgradeName;
        public string Description;
        public Sprite Icon;
        public virtual void ApplyUpgrade()
        {
            Debug.Log(string.Format("Upgrade applied, Name: {0}", UpgradeName));
        }
        public virtual bool CanUpgrade()
        {
            return true;
        }
        [Button]
        public void RebuildId()
        {
            this.id = string.Join("_", this.name.Trim().ToLower().Replace(" ", "_"), this.Rarity.ToString().ToLower());
        }
#if UNITY_EDITOR
        [Button]
        public void CopyIdToClipboard()
        {
            GUIUtility.systemCopyBuffer = Id;
        }
#endif
    }
}
