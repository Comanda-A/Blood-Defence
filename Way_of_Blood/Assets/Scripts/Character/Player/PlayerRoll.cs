using UnityEngine;
using WayOfBlood.Input;

namespace WayOfBlood.Character.Player
{
    public class PlayerRoll : CharacterRoll
    {
        private InputBase playerInputSystem;

        protected override void Start()
        {
            base.Start();
            playerInputSystem = GetComponent<InputBase>();
        }

        private void Update()
        {
            if (playerInputSystem.RollKeyPressed)
            {
                Roll();
            }
        }
    }
}