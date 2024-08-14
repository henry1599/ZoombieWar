using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance {get; private set;}
        [SerializeField, Required] CaveGenerator caveGenerator;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        void Start()
        {
            GenerateMap();
        }
        [Button]
        public void GenerateMap()
        {
            caveGenerator.Generate(PlayerEvents.OnMapGenerated);
        }
        public Cell GetSpawnCell()
        {
            return this.caveGenerator?.MainRoom?.GetCenterCell();
        }
    }
}
