using System.Collections;
using System.Collections.Generic;
using Pooling;
using UnityEngine;

namespace Survival
{
    public class PooledVFX : MonoBehaviour
    {
        void OnParticleSystemStopped()
        {
            gameObject.Despawn();
        }
    }
}
