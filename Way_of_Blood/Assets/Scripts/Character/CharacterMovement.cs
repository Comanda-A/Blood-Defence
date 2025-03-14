using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        /// <summary>
        /// ���������� ������ ��� ��� ��������� ������� ����������� ������� (Vector2 ViewDirection).
        /// </summary>
        public event UnityAction<Vector2> OnViewDirectionChanged;

        /// <summary>
        /// ���������� ������ ��� ��� ��������� ������� ����������� (Vector2 MoveDirection).
        /// </summary>
        public event UnityAction<Vector2> OnMoveDirectionChanged;

        /// <summary>
        /// ���������� ��� ����������� �������� (MoveDirection != Vector2.Zero).
        /// </summary>
        public event UnityAction<Vector2> OnMoving;

        [Header("Movement parameters")]
        public float DefaultSpeed;              // �������� �� ���������
        public float DefaultAcceleration;       // ��������� �� ���������
        public float CurrentSpeed;              // ��������
        public float CurrentAcceleration;       // ���������

        private bool _closedViewDirection;              // ������� �� ��������� ����������� ��������� (��������, ������������)
        private Vector2 _viewDirection = Vector2.zero;  // ViewDirection

        /// <summary>
        /// ����������� �������.
        /// </summary>
        public Vector2 ViewDirection
        {
            protected set
            {
                if (value != _viewDirection && !_closedViewDirection)
                {
                    _viewDirection = value.normalized;
                    OnViewDirectionChanged?.Invoke(value);
                }
            }

            get { return _viewDirection; }
        }

        private bool _closedMoveDirection;              // ������� �� ��������� ����������� �������� (��������, ������������)
        private Vector2 _moveDirection = Vector2.zero;  // MoveDirection

        /// <summary>
        /// ����������� ��������.
        /// </summary>
        public Vector2 MoveDirection
        {
            protected set
            {
                if (value != _moveDirection && !_closedMoveDirection)
                {
                    _moveDirection = value.normalized;
                    OnMoveDirectionChanged?.Invoke(value);
                }
            }

            get { return _moveDirection; }
        }

        protected Rigidbody2D _rigidbody2D;

        // ���������� ����������� (��������, ���������)
        // ��������� ��������� ��� ���������� ����������� �����������
        private bool _defaultOnCompletionForcedMovement;
        private float _forcedMovementTime;           // ����� ������� ���������
        private Coroutine _forcedMovementCoroutine;  // ��������

        protected virtual void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _closedMoveDirection = false;
            _closedViewDirection = false;
            SetDefaultParameters();
        }

        protected virtual void Update()
        {
            if (MoveDirection != Vector2.zero)
                OnMoving?.Invoke(MoveDirection);
        }

        protected virtual void FixedUpdate()
        {
            Vector2 targetVelocity = MoveDirection * CurrentSpeed;
            _rigidbody2D.linearVelocity = Vector2.Lerp(
                _rigidbody2D.linearVelocity, targetVelocity, Time.fixedDeltaTime * CurrentAcceleration);
        }

        /// <summary>
        /// ������������� ������ � ���������� MoveDirection. true - ��������� ��������, false - ��������� �������
        /// </summary>
        /// <param name="value"></param>
        public void SetMoveDirectionForChanges(bool value, Vector2 moveDirection)
        {
            MoveDirection = moveDirection;
            _closedMoveDirection = !value;
        }

        /// <summary>
        /// ���������� ���������� �������� � ������������ �����������.
        /// </summary>
        /// <param name="move">����������� �����������</param>
        /// <param name="view">����������� �������</param>
        /// <param name="time">����� ����������� (������������)</param>
        /// <param name="speed">��������</param>
        /// <param name="acceleration">���������</param>
        /// <param name="defaultOnCompletion">������� ��������� ������� ��� ����������</param>
        public void SetForcedMovement(
            Vector2 move,
            Vector2 view,
            float time,
            float speed, 
            float acceleration,
            bool defaultOnCompletion = true)
        {
            MoveDirection = move;
            ViewDirection = view;
            CurrentSpeed = speed;
            CurrentAcceleration = acceleration;
            _forcedMovementTime = time;
            _defaultOnCompletionForcedMovement = defaultOnCompletion;
            _closedMoveDirection = true;
            _closedViewDirection = true;
            _forcedMovementCoroutine = StartCoroutine(ForcedMovementHandler());
        }

        public void SetDefaultParameters()
        {
            CurrentSpeed = DefaultSpeed;
            CurrentAcceleration = DefaultAcceleration;
            MoveDirection = Vector2.zero;
            ViewDirection = Vector2.down;
        }

        private IEnumerator ForcedMovementHandler()
        {
            yield return new WaitForSeconds(_forcedMovementTime);

            _closedMoveDirection = false;
            _closedViewDirection = false;

            if (_defaultOnCompletionForcedMovement)
                SetDefaultParameters();

            yield break;
        }

        protected virtual void OnDestroy()
        {
            OnViewDirectionChanged = null;
            OnMoveDirectionChanged = null;
            OnMoving = null;
        }
    }
}
