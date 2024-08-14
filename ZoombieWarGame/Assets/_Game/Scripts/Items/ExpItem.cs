using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public enum eExpType
    {
        Small,
        Medium,
        Large
    }
    public class ExpItem : Item
    {
        [SerializeField] eExpType expType;
        [SerializeField] float expValue = 1;
        public eExpType ExpType => expType;
        protected override void OnCollect()
        {
            base.OnCollect();
            PlayerEvents.OnExpCollected?.Invoke(expValue);
        }
    }
}
