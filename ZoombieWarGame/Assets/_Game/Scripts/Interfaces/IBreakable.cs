using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public interface IBreakable
    {
        /// <summary>
        /// Value is decrease to 0
        /// </summary>
        event System.Action OnBroken;
        /// <summary>
        /// Has value been changed and how much?
        /// </summary>
        event System.Action<int> OnValueChanged;
        void Init(int miningLimit);
        // * Call when an entity try to mine this tile
        void Mine(int miningValue);
        int GetCurrentValue();
    }
}
