using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }
        [SerializeField] string[] itemPaths;
        Dictionary<eResourceId, ResourceItem> resourceData;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            LoadResources();
        }

        public void LoadResources()
        {
            this.resourceData = new();
            foreach (string path in itemPaths)
            {
                var data = Resources.Load(path);
                if (data == null)
                    continue;
                var resourceItem = (data as GameObject)?.GetComponent<ResourceItem>();
                if (resourceItem == null)
                    continue;
                this.resourceData.TryAdd(resourceItem.Id, resourceItem);
            }
        }
        public ResourceItem GetResourceData(eResourceId id)
        {
            if (this.resourceData.ContainsKey(id))
            {
                return this.resourceData[id];
            }
            return null;
        }
    }
}
