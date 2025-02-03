using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace WayOfBlood.Character.Player
{
    public class PlayerMovement : CharacterMovement
    {
        private InputAction moveAction;

        protected override void Start()
        {
            base.Start();
            moveAction = InputSystem.actions.FindAction("Move");
        }

        protected void Update()
        {
            MoveDirection = moveAction.ReadValue<Vector2>();

            if (MoveDirection != Vector2.zero && Mathf.Abs(MoveDirection.x) != Mathf.Abs(MoveDirection.y))
            {
                // Определяем новое направление взгляда
                if (Mathf.Abs(MoveDirection.x) >= Mathf.Abs(MoveDirection.y))
                {
                    ViewDirection = MoveDirection.x > 0 ? Vector2.right : Vector2.left;
                }
                else
                {
                    ViewDirection = MoveDirection.y > 0 ? Vector2.up : Vector2.down;
                }
            }
        }
    }
}