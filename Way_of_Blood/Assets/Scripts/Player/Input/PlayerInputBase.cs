using UnityEngine;

namespace WayOfBlood.Player.Input
{
    public abstract class PlayerInputBase : MonoBehaviour
    {
        public abstract Vector2 MoveDirection { get; }

        public abstract bool AttackKeyPressed { get; }

        public abstract bool ShotKeyPressed { get; }

        public abstract bool RollKeyPressed { get; }

        public abstract bool InteractionKeyPressed { get; }

        public static PlayerInputBase GetCurrentInput()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return new PlayerInputPC();
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            return new PlayerInputTouch();
#else
            return new PlayerInputJoystick();
#endif
        }
    }
}