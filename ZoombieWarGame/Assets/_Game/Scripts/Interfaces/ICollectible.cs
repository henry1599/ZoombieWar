using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public interface ICollectible
    {
        void Collect(GameObject target);
    }
}
