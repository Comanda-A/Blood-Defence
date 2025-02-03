using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        public event UnityAction<Vector2> OnViewDirectionChanged;
        public event UnityAction<Vector2> OnMoveDirectionChanged;

        [Header("Movement parameters")]
        public float DefaultSpeed;              // �������� �� ���������
        public float DefaultAcceleration;       // ��������� �� ���������
        public float CurrentSpeed;              // ��������
        public float CurrentAcceleration;       // ���������

        private bool _closedViewDirection;  // ������� �� ��������� ����������� ��������� (��������, ������������)
        private Vector2 _viewDirection;
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

        private bool _closedMoveDirection; // ������� �� ��������� ����������� �������� (��������, ������������)
        private Vector2 _moveDirection;
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

        protected new Rigidbody2D rigidbody2D;

        private bool _defaultOnCompletion;          // ������ ��� ����������
        private float _closedChangesTime;           // ����� ������� ���������
        private Coroutine _closedChangesCoroutine;  // ��������

        protected virtual void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            _closedMoveDirection = false;
            _closedViewDirection = false;
            SetDefaultParameters();
        }

        protected virtual void FixedUpdate()
        {
            Vector2 targetVelocity = MoveDirection * CurrentSpeed;
            rigidbody2D.linearVelocity = Vector2.Lerp(
                rigidbody2D.linearVelocity, targetVelocity, Time.fixedDeltaTime * CurrentAcceleration);
        }

        public void SetConstantDirection(       // ���������� ��������
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
            _closedChangesTime = time;
            _defaultOnCompletion = defaultOnCompletion;
            _closedMoveDirection = true;
            _closedViewDirection = true;
            _closedChangesCoroutine = StartCoroutine(CloseChangeDirectionsHandler());
        }

        public void SetDefaultParameters()
        {
            CurrentSpeed = DefaultSpeed;
            CurrentAcceleration = DefaultAcceleration;
            MoveDirection = Vector2.zero;
            ViewDirection = Vector2.down;
        }

        private IEnumerator CloseChangeDirectionsHandler()
        {
            yield return new WaitForSeconds(_closedChangesTime);

            _closedMoveDirection = false;
            _closedViewDirection = false;

            if (_defaultOnCompletion)
                SetDefaultParameters();

            yield break;
        }

        protected virtual void OnDestroy()
        {
            OnViewDirectionChanged = null;
            OnMoveDirectionChanged = null;
        }
    }
}
