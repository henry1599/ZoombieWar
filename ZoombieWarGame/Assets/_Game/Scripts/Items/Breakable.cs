using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public class Breakable : MonoBehaviour, IBreakable
    {
        [SerializeField, ReadOnly] int miningLimit;

        public event Action OnBroken;
        public event Action<int> OnValueChanged;

        public int GetCurrentValue()
        {
            return this.miningLimit;
        }

        public void Init(int miningLimit)
        {
            this.miningLimit = miningLimit;
        }

        public void Mine(int miningValue)
        {
            this.miningLimit -= miningValue;
            this.OnValueChanged?.Invoke(miningValue);
            if (this.miningLimit <= 0)
            {
                OnBroken?.Invoke();
            }
        }
    }
}
