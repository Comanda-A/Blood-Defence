using UnityEngine;
using WayOfBlood.Settings;

namespace WayOfBlood.Player.Input
{
    public class PlayerInputPC : PlayerInputBase
    {
        public override Vector2 MoveDirection
        {
            get
            {
                float horizontal = 0;
                float vertical = 0;

                if (UnityEngine.Input.GetKey(InputSettings.MoveLeft)) horizontal -= 1;
                if (UnityEngine.Input.GetKey(InputSettings.MoveRight)) horizontal += 1;
                if (UnityEngine.Input.GetKey(InputSettings.MoveUp)) vertical += 1;
                if (UnityEngine.Input.GetKey(InputSettings.MoveDown)) vertical -= 1;

                return new Vector2(horizontal, vertical).normalized;
            }
        }

        public override bool AttackKeyPressed
        {
            get
            {
                return UnityEngine.Input.GetKeyDown(InputSettings.AttackKey);
            }
        }

        public override bool ShotKeyPressed
        {
            get
            {
                return UnityEngine.Input.GetKeyDown(InputSettings.ShotKey);
            }
        }

        public override bool RollKeyPressed
        {
            get
            {
                return UnityEngine.Input.GetKeyDown(InputSettings.RollKey);
            }
        }

        public override bool InteractionKeyPressed
        {
            get
            {
                return UnityEngine.Input.GetKeyDown(InputSettings.InteractionKey);
            }
        }
    }
}