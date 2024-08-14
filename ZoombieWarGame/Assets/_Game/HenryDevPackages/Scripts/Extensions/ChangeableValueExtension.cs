using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HenryDev
{
    public static class ChangeableValueExtension
    {
        // * Health
        public static IChangeableValue GetChangeableComponent<T>(this Component target) where T : IChangeableValue
        {
            var result = target.gameObject.GetComponents<IChangeableValue>();
            if (result == null)
            {
                Logger.LogError(string.Format("Health component not found in object {0}", target.gameObject.name));
                return null;
            }
            foreach (var item in result)
            {
                if (item is T)
                {
                    return item;
                }
            }
            return null;
        }
        public static IChangeableValue GetChangeableComponent<T>(this GameObject target) where T : IChangeableValue
        {
            var result = target.GetComponents<IChangeableValue>();
            if (result == null)
            {
                Logger.LogError(string.Format("Health component not found in object {0}", target.gameObject.name));
                return null;
            }
            foreach (var item in result)
            {
                if (item is T)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
