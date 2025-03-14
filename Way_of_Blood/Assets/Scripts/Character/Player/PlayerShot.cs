using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WayOfBlood.Character.Player
{
    public class PlayerShot : CharacterShot
    {
        [Header("Shooting settings")]
        public float BulletRadiusAtShot = 0.8f;     // Радиус стрельбы

        [Header("Auto aiming")]
        public float AimAssistRadius = 15f;         // Радиус помощи прицеливания
        public float AimAssistAngle = 45f;          // Угол помощи прицеливания
        public float AimLockStrength = 10f;        // Сила доводки прицела

        private Transform _transform;
        private Camera _mainCamera;
        private PlayerMovement _playerMovement;
        private PlayerBlood _playerBlood;

        private InputAction _shotAction;
        private InputAction _сhangeShootingModeAction;

        private bool _aimMode;  // Режим прицеливания

        void Start()
        {
            _mainCamera = Camera.main;
            _transform = GetComponent<Transform>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerBlood = GetComponent<PlayerBlood>();

            _shotAction = InputSystem.actions.FindAction("Shot");
            _shotAction.performed += ShotHandler;

            _сhangeShootingModeAction = InputSystem.actions.FindAction("ChangeShootingMode");
            _сhangeShootingModeAction.performed += ChangeShootingModeHandler;

            _aimMode = false;
        }

        private void Update()
        {
            if (_aimMode)
            {
                Transform autoAimTarget = GetAutoAimTarget(_playerMovement.GetMoveDirectionInput());

                if (autoAimTarget != null)
                {
                    _playerMovement.ViewDirectionSetting = PlayerMovement.ViewDirectionMode.AutoAim;
                    _playerMovement.SetAutoAimTarget(autoAimTarget, AimLockStrength);
                }
                else
                {
                    _playerMovement.ViewDirectionSetting = PlayerMovement.ViewDirectionMode.Free;
                }
            }
        }

        private void ShotHandler(InputAction.CallbackContext context)
        {
            if (Time.time < lastShotTime + ShotCooldown || _playerBlood.Blood == 0)
                return;

            _playerBlood.TakeBlood(1);
            Shot((Vector2)_transform.position + _playerMovement.ViewDirection * BulletRadiusAtShot, _playerMovement.ViewDirection);
        }

        private void ChangeShootingModeHandler(InputAction.CallbackContext context)
        {
            if (_aimMode)
            {
                _playerMovement.SetMoveDirectionForChanges(true, Vector2.zero);
                _playerMovement.ViewDirectionSetting = PlayerMovement.ViewDirectionMode.FourDirections;
            }
            else
            {
                _playerMovement.SetMoveDirectionForChanges(false, Vector2.zero);
                _playerMovement.ViewDirectionSetting = PlayerMovement.ViewDirectionMode.Free;
            }
                
            _aimMode = !_aimMode;
        }

        private Transform GetAutoAimTarget(Vector2 viewDirection)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(_transform.position, AimAssistRadius, LayerMask.GetMask("Enemy"));

            Transform bestTarget = null;
            float closestAngle = AimAssistAngle / 2;
            float closestDistance = Mathf.Infinity;  // Изначально устанавливаем бесконечное расстояние

            foreach (var enemy in enemies)
            {
                Vector2 directionToEnemy = (enemy.transform.position - _transform.position).normalized;
                float angle = Vector2.Angle(viewDirection, directionToEnemy);

                // Проверяем, находится ли враг в пределах допустимого угла
                if (angle < closestAngle)
                {
                    // Если враг в пределах угла, проверяем его расстояние
                    float distance = Vector2.Distance(_transform.position, enemy.transform.position);

                    // Если враг ближе, обновляем лучший результат
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        bestTarget = enemy.transform;
                    }
                }
            }

            return bestTarget;
        }


        private void OnDestroy()
        {
            _shotAction.performed -= ShotHandler;
        }
    }
}
