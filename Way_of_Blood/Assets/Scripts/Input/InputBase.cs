using UnityEngine;

namespace WayOfBlood.Input
{
    public abstract class InputBase : MonoBehaviour
    {
        public abstract Vector2 MoveDirection { get; }

        public abstract bool AttackKeyPressed { get; }

        public abstract bool ShotKeyPressed { get; }

        public abstract bool RollKeyPressed { get; }

        public abstract bool InteractionKeyPressed { get; }

        public static InputBase GetCurrentInput()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return new InputPC();
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            return new InputTouch();
#else
            return new InputJoystick();
#endif
        }
    }
}