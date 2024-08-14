using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HenryDev
{
    public interface IChangeableValue
    {
        /// <summary>
        /// Value is decrease to 0
        /// </summary>
        event System.Action OnEmpty;
        /// <summary>
        /// Has value been changed and how much?
        /// </summary>
        event System.Action<float> OnValueChanged;

        void InitValue(float value, bool startFrom0 = true);
        void ForceInitValue(float value, bool startFrom0 = true);
        void UpdateValue(float value);
        void MakeEmpty();
        void UpdateMaxValue(float maxValue);

        float GetMaxValue();
        float GetValue();
        float GetValueNormalized();
    }
}
