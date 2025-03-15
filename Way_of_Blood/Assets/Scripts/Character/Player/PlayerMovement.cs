using UnityEngine;
using UnityEngine.InputSystem;
using System;
using WayOfBlood.ControlInputSystem;

namespace WayOfBlood.Character.Player
{
    public class PlayerMovement : CharacterMovement
    {
        [Header("View direction mode")]
        public ViewDirectionMode ViewDirectionSetting = ViewDirectionMode.FourDirections;

        private InputAction _moveAction;
        private ControlInput _controlInput;
        private Joystick _joystick;
        private Transform _transform;
        private Transform _autoAimTarget;

        private float _aimLockStrength = 0.5f;  // ���� ������� �������

        protected override void Start()
        {
            base.Start();

            _transform = GetComponent<Transform>();
            _controlInput = GetComponent<ControlInput>();
            _moveAction = InputSystem.actions.FindAction("Move");
            _joystick = GameObject.FindGameObjectWithTag("Joystick")?.GetComponent<Joystick>();
        }

        protected override void Update()
        {
            MoveDirection = GetMoveDirectionInput();
            ViewDirection = CalculateViewDirection(GetViewDirectionInput());

            base.Update();
        }

        public Vector2 CalculateViewDirection(Vector2 viewDirection)
        {
            switch (ViewDirectionSetting)
            {
                case ViewDirectionMode.FourDirections:
                    return (viewDirection != Vector2.zero) ? Get4DirectionView(viewDirection) : ViewDirection;
                case ViewDirectionMode.EightDirections:
                    return (viewDirection != Vector2.zero) ? Get8DirectionView(viewDirection) : ViewDirection;
                case ViewDirectionMode.Free:
                    return (viewDirection != Vector2.zero) ? viewDirection.normalized : ViewDirection;
                case ViewDirectionMode.AutoAim:
                    return (_autoAimTarget != null) ? GetAutoAimDirection(_autoAimTarget) : ViewDirection;
                default:
                    return ViewDirection;
            }
        }

        private Vector2 Get4DirectionView(Vector2 moveDirection)
        {
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                return moveDirection.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                return moveDirection.y > 0 ? Vector2.up : Vector2.down;
            }
        }

        private Vector2 Get8DirectionView(Vector2 moveDirection)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            if (angle >= 22.5f && angle < 67.5f) return new Vector2(1, 1).normalized; // ����-�����
            if (angle >= 67.5f && angle < 112.5f) return Vector2.up; // ����
            if (angle >= 112.5f && angle < 157.5f) return new Vector2(-1, 1).normalized; // ����-����
            if (angle >= 157.5f && angle < 202.5f) return Vector2.left; // �����
            if (angle >= 202.5f && angle < 247.5f) return new Vector2(-1, -1).normalized; // ���-����
            if (angle >= 247.5f && angle < 292.5f) return Vector2.down; // ����
            if (angle >= 292.5f && angle < 337.5f) return new Vector2(1, -1).normalized; // ���-�����
            return Vector2.right; // ������
        }

        private Vector2 GetAutoAimDirection(Transform target)
        {
            if (target == null) return ViewDirection;

            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Vector2 targetDirection = new Vector2(directionToTarget.x, directionToTarget.y);

            // ������� ��������� ����������� � ����
            return Vector2.Lerp(ViewDirection, targetDirection, _aimLockStrength * Time.deltaTime).normalized;
        }

        public void SetAutoAimTarget(Transform target, float aimLockStrength)
        {
            _autoAimTarget = target;
            _aimLockStrength = aimLockStrength;
        }

        /// <summary>
        /// ���� ����������� �������� (InputAction->MoveAction).
        /// </summary>
        /// <returns>����������� ��������</returns>
        public Vector2 GetMoveDirectionInput()
        {
            switch(_controlInput.CurrentInputType)
            {
                case ControlInput.InputType.Keyboard:
                case ControlInput.InputType.Gamepad:
                    return _moveAction.ReadValue<Vector2>().normalized;
                case ControlInput.InputType.Touch:
                    return _joystick.Direction.normalized;
                default:
                    return Vector2.zero;
            }
        }

        public Vector2 GetViewDirectionInput()
        {
            Vector2 direction = Vector2.zero;

            switch (_controlInput.CurrentInputType)
            {
                case ControlInput.InputType.Keyboard:
                case ControlInput.InputType.Gamepad:
                case ControlInput.InputType.Touch:
                    return MoveDirection;
                default:
                    return Vector2.zero;
            }
        }


        [Serializable]
        public enum ViewDirectionMode
        {
            /// <summary>
            /// �������������� ��������� �� ����������� �����
            /// </summary>
            AutoAim,

            /// <summary>
            /// 4 ����������� (�����, ����, �����, ������)
            /// </summary>
            FourDirections,

            /// <summary>
            /// 8 ����������� (������� ���������)
            /// </summary>
            EightDirections,

            /// <summary>
            /// ������ ������� (����� �����������)
            /// </summary>
            Free
        }
    }
}
