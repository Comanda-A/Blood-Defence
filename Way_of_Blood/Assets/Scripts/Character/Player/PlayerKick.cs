using UnityEngine;
using UnityEngine.InputSystem;

namespace WayOfBlood.Character.Player
{
    public class PlayerKick : ÑharacterKick
    {
        private InputAction kickAction;

        protected override void Start()
        {
            base.Start();
            kickAction = InputSystem.actions.FindAction("Kick");
            kickAction.performed += KickHandler;
        }

        private void KickHandler(InputAction.CallbackContext context) => Kick();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            kickAction.performed -= KickHandler;
        }
    }
}
