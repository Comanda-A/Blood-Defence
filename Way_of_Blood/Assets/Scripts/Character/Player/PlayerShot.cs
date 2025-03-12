using UnityEngine;
using UnityEngine.InputSystem;

namespace WayOfBlood.Character.Player
{
    public class PlayerShot : CharacterShot
    {
        public float ShotRadius = 1f;

        private new Transform transform;
        private Camera mainCamera;
        private CharacterMovement _characterMovement;
        private CharacterBlood _characterBlood;
        private InputAction shotAction;

        void Start()
        {
            transform = GetComponent<Transform>();
            mainCamera = Camera.main;
            _characterMovement = GetComponent<CharacterMovement>();
            _characterBlood = GetComponent<CharacterBlood>();
            shotAction = InputSystem.actions.FindAction("Shot");
            shotAction.performed += ShotHandler;
        }

        private void ShotHandler(InputAction.CallbackContext context)
        {
            if (Time.time < lastShotTime + ShotCooldown || _characterBlood.Blood == 0)
                return;
            
            Vector2 direction = Vector2.zero;

            if (context.control.device is Mouse)
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                direction = ((Vector2)(mousePosition - transform.position)).normalized;
            }
            else
            {
                direction = _characterMovement.ViewDirection;
            }

            _characterBlood.TakeBlood(1);
            Shot((Vector2)transform.position + direction * ShotRadius, direction);
        }

        private void OnDestroy()
        {
            shotAction.performed -= ShotHandler;
        }
    }
}