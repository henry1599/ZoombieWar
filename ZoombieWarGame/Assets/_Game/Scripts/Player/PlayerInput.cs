using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival
{
    public class PlayerInput : MonoBehaviour, IInput
    {
        Joystick joystick;
        public FrameInput Input { get; set; }
        void Awake()
        {
            this.joystick = FindObjectOfType<Joystick>();
        }
        private void Update() 
        {
            if (this.joystick == null)
            {
                Debug.LogError("Joystick not found");
                return;
            }
            Input = Gather();
        }
        FrameInput Gather()
        {
            return new FrameInput 
            {
#if UNITY_EDITOR
                Move = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical")).normalized
#else
                Move = this.joystick.Direction.normalized
#endif
            };
        }
    }
    public class FrameInput
    {
        public Vector2 Move;
    }
}
