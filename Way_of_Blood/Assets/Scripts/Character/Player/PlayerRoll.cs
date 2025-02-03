using UnityEngine.InputSystem;

namespace WayOfBlood.Character.Player
{
    public class PlayerRoll : CharacterRoll
    {
        private InputAction rollAction;

        protected override void Start()
        {
            base.Start();
            rollAction = InputSystem.actions.FindAction("Roll");
            rollAction.performed += RollHandler;
        }

        private void RollHandler(InputAction.CallbackContext context) => Roll();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            rollAction.performed -= RollHandler;
        }
    }
}