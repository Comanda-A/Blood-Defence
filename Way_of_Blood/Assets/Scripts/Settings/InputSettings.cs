using UnityEngine;
using System.IO;
using System;

namespace WayOfBlood.Settings
{
    [System.Serializable]
    public static class InputSettings
    {
        // Клавиши для управления движением
        public static KeyCode MoveUp = KeyCode.W;
        public static KeyCode MoveDown = KeyCode.S;
        public static KeyCode MoveLeft = KeyCode.A;
        public static KeyCode MoveRight = KeyCode.D;

        // Клавиши для действий
        public static KeyCode AttackKey = KeyCode.Mouse0;
        public static KeyCode ShotKey = KeyCode.Mouse1;
        public static KeyCode RollKey = KeyCode.LeftShift;
        public static KeyCode InteractionKey = KeyCode.E;

        // Оси для джойстика
        public static string JoystickHorizontalAxis = "Horizontal";
        public static string JoystickVerticalAxis = "Vertical";
    }
}