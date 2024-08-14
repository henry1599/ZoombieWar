using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public abstract class Tile : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;
        protected Cell cellData;
        public Cell CellData => this.cellData;
        public virtual void Setup(Cell cellData, Sprite sprite)
        {
            this.cellData = cellData;
            this.spriteRenderer.sprite = sprite;
        }
    }
}
