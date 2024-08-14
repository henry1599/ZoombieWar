using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Survival
{
    public class PlayerAnimator : MonoBehaviour
    {
        private readonly int MoveKey = Animator.StringToHash("Move");
        [SerializeField] Animator animator;
        [SerializeField] Transform graphic;
        IController controller;
        void Awake()
        {
            this.controller = GetComponent<IController>();
        }
        void OnEnable()
        {
            this.controller.OnMove += HandleMove;
        }
        void OnDestroy()
        {
            this.controller.OnMove -= HandleMove;
        }
        private void HandleMove(Vector2 vector)
        {
            bool isMoving = vector.sqrMagnitude > 0;
            this.animator.SetBool(MoveKey, isMoving);
            if (!isMoving)
                return;
            Flip(vector.x);
        }
        private void Flip(float xValue)
        {
            var scale = this.graphic.localScale;
            scale = new Vector3(Mathf.Sign(xValue) * Mathf.Abs(scale.x), scale.y, scale.z);
            this.graphic.localScale = scale;
        }

    }
}
