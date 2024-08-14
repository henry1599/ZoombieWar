using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public abstract class Ability : MonoBehaviour
    {
        public bool IsEnabled { get; protected set; }
        public abstract void Run();
        public abstract void RunPhysics();
        public virtual void SetEnable(bool value)
        {
            IsEnabled = value;
        }
        protected virtual void Update()
        {
            if (!IsEnabled)
                return;
            Run();
        }
        protected virtual void FixedUpdate()
        {
            if (!IsEnabled)
                return;
            RunPhysics();
        }
    }
}
