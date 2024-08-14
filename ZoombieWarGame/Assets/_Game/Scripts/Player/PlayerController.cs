using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public class PlayerController : MonoBehaviour, IController
    {
        IInput input;
        [SerializeField] float movementSpeed;
        [SerializeField] Rigidbody2D rb2D;

        public IInput Input => this.input;

        public event Action<Vector2> OnMove;
        public event Action OnAttack;
        void Awake()
        {
            this.input = GetComponent<IInput>();
        }
        void Update()
        {
            this.rb2D.velocity = this.input.Input.Move * this.movementSpeed;
            this.OnMove?.Invoke(this.input.Input.Move);
        }
    }
}
