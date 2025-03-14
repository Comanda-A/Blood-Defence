using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WayOfBlood.Character.Player
{
    public class PlayerShot : CharacterShot
    {
        [Header("Shooting settings")]
        public float BulletRadiusAtShot = 0.8f;     // ������ ��������

        [Header("Auto aiming")]
        public float AimAssistRadius = 15f;         // ������ ������ ������������
        public float AimAssistAngle = 45f;          // ���� ������ ������������
        public float AimLockStrength = 10f;        // ���� ������� �������

        private Transform _transform;
        private Camera _mainCamera;
        private PlayerMovement _playerMovement;
        private PlayerBlood _playerBlood;

        private InputAction _shotAction;
        private InputAction _�hangeShootingModeAction;

        private bool _aimMode;  // ����� ������������

        void Start()
        {
            _mainCamera = Camera.main;
            _transform = GetComponent<Transform>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerBlood = GetComponent<PlayerBlood>();

            _shotAction = InputSystem.actions.FindAction("Shot");
            _shotAction.performed += ShotHandler;

            _�hangeShootingModeAction = InputSystem.actions.FindAction("ChangeShootingMode");
            _�hangeShootingModeAction.performed += ChangeShootingModeHandler;

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
            float closestDistance = Mathf.Infinity;  // ���������� ������������� ����������� ����������

            foreach (var enemy in enemies)
            {
                Vector2 directionToEnemy = (enemy.transform.position - _transform.position).normalized;
                float angle = Vector2.Angle(viewDirection, directionToEnemy);

                // ���������, ��������� �� ���� � �������� ����������� ����
                if (angle < closestAngle)
                {
                    // ���� ���� � �������� ����, ��������� ��� ����������
                    float distance = Vector2.Distance(_transform.position, enemy.transform.position);

                    // ���� ���� �����, ��������� ������ ���������
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
