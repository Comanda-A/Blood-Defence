using UnityEngine;
using UnityEngine.InputSystem;

namespace WayOfBlood.Character.Player
{
    public class PlayerShot : CharacterShot
    {
        public float ShotRadius = 1f;

        private new Transform transform;
        private Camera mainCamera;
        private CharacterMovement characterMovement; 
        private InputAction shotAction;

        void Start()
        {
            transform = GetComponent<Transform>();
            mainCamera = Camera.main;
            characterMovement = GetComponent<CharacterMovement>();
            shotAction = InputSystem.actions.FindAction("Shot");
            shotAction.performed += ShotHandler;
        }

        private void ShotHandler(InputAction.CallbackContext context)
        {
            if (Time.time < lastShotTime + ShotCooldown)
                return;
            
            Vector2 direction = Vector2.zero;

            if (context.control.device is Mouse)
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                direction = ((Vector2)(mousePosition - transform.position)).normalized;
            }
            else
            {
                direction = characterMovement.ViewDirection;
            }

            Shot((Vector2)transform.position + direction * ShotRadius, direction);
            
        }

        private void OnDestroy()
        {
            shotAction.performed -= ShotHandler;
        }
    }
}