using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Input;

namespace WayOfBlood.Character.Player
{
    public class PlayerMovement : CharacterMovement
    {
        private InputBase playerInputSystem;

        protected override void Start()
        {
            base.Start();
            playerInputSystem = GetComponent<InputBase>();
        }

        protected void Update()
        {
            MoveDirection = playerInputSystem.MoveDirection;

            if (MoveDirection != Vector2.zero)
            {
                // Проверяем, совпадает ли текущее направление взгляда с направлением движения
                bool isSameDirection =
                    (ViewDirection == Vector2.up && MoveDirection.y > 0) ||
                    (ViewDirection == Vector2.down && MoveDirection.y < 0) ||
                    (ViewDirection == Vector2.right && MoveDirection.x > 0) ||
                    (ViewDirection == Vector2.left && MoveDirection.x < 0);

                if (!isSameDirection)
                {
                    // Обновляем направление взгляда
                    ViewDirection = MoveDirection.x != 0
                        ? (MoveDirection.x > 0 ? Vector2.right : Vector2.left)
                        : (MoveDirection.y > 0 ? Vector2.up : Vector2.down);
                }
            }
        }
    }
}