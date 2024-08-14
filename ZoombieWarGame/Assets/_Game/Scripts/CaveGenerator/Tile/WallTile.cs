using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public class WallTile : Tile
    {
        private readonly string DISSOLVE_AMOUNT = "_DissolveAmount";
        private readonly string DISSOLVE_OFFSET = "_NoiseOffset";
        private readonly string FRAGILE_LEVEL = "_FragileLevel";
        [SerializeField] bool isBreakable;
        [SerializeField] int maxMiningLimit;
        [MinMaxSlider(0f, 1f), SerializeField] Vector2 dissolveRange;
        [MinMaxSlider(0f, 100f), SerializeField] Vector2 dissolveOffsetRange;
        [Range(0.5f, 3f), SerializeField] float fragileLevel;
        private IBreakable breakable;
        private Material material;
        public override void Setup(Cell cellData, Sprite sprite)
        {
            base.Setup(cellData, sprite);
            this.breakable = GetComponent<IBreakable>();
            this.breakable.Init(this.maxMiningLimit);
            SetupMaterial();
            SetupDissolveShader();

            this.breakable.OnBroken += HandleBroken;
            this.breakable.OnValueChanged += HandleValueChanged;
        }
        void OnDestroy()
        {
            this.breakable.OnBroken -= HandleBroken;
            this.breakable.OnValueChanged -= HandleValueChanged;
        }

        private void HandleValueChanged(int value)
        {
            float currentValue = this.breakable.GetCurrentValue();
            float remaining = this.maxMiningLimit - currentValue;
            float remaningPercent = remaining / this.maxMiningLimit;
            float dissolveAmount = (this.dissolveRange.y - this.dissolveRange.x) * remaningPercent + this.dissolveRange.x;

            this.material.SetFloat(DISSOLVE_AMOUNT, dissolveAmount);
        }

        private void HandleBroken()
        {
            Destroy(gameObject);
        }
        private void SetupMaterial()
        {
            this.material = new Material(this.spriteRenderer.material);
            this.spriteRenderer.material = this.material;
        }
        private void SetupDissolveShader()
        {
            this.material.SetFloat(DISSOLVE_AMOUNT, this.dissolveRange.x);

            var randomOffset = UnityEngine.Random.Range(dissolveOffsetRange.x, this.dissolveOffsetRange.y);
            this.material.SetVector(DISSOLVE_OFFSET, new Vector4(randomOffset, randomOffset, 0, 0));
            this.material.SetVector(FRAGILE_LEVEL, new Vector4(this.fragileLevel, this.fragileLevel, 0, 0));
        }
    }
}
