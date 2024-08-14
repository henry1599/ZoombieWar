using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public interface IController
    {
        public event Action<Vector2> OnMove;
        public event Action OnAttack;
    }
}