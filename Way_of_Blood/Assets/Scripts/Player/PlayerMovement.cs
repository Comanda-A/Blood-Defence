using System;
using UnityEngine;
using WayOfBlood.Player.Input;

namespace WayOfBlood.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float Speed;
        public float Acceleration;

        public event UnityEngine.Events.UnityAction<Vector2> OnViewDirectionChanged;
        public event UnityEngine.Events.UnityAction<Vector2> OnMoving;

        private Vector2 _viewDirection;
        public Vector2 ViewDirection
        {
            private set
            {
                if (value != _viewDirection)
                {
                    _viewDirection = value;
                    OnViewDirectionChanged?.Invoke(value);
                }
            }

            get { return _viewDirection; }
        }

        public Vector2 MoveDirection { private set; get; }


        private PlayerInputBase playerInputSystem;
        private new Rigidbody2D rigidbody2D;


        private void Start()
        {
            playerInputSystem = GetComponent<PlayerInputBase>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            MoveDirection = playerInputSystem.MoveDirection;

            if (MoveDirection != Vector2.zero)
            {
                if (MoveDirection.x != 0)
                    ViewDirection = MoveDirection.x > 0 ? Vector2.right : Vector2.left;
                else
                    ViewDirection = MoveDirection.y > 0 ? Vector2.up : Vector2.down;

                OnMoving?.Invoke(MoveDirection);
            }
        }

        private void FixedUpdate()
        {
            Vector2 targetVelocity = MoveDirection * Speed;
            rigidbody2D.linearVelocity = Vector2.Lerp(
                rigidbody2D.linearVelocity, targetVelocity, Time.fixedDeltaTime * Acceleration);
        }
    }
}