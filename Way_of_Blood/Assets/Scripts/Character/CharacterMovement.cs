using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace WayOfBlood.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        public event UnityAction<Vector2> OnViewDirectionChanged;
        public event UnityAction<Vector2> OnMoveDirectionChanged;

        [Header("Movement parameters")]
        public float DefaultSpeed;              // Скорость по умолчанию
        public float DefaultAcceleration;       // Ускорение по умолчанию
        public float CurrentSpeed;              // Скорость
        public float CurrentAcceleration;       // Ускорение

        private bool _closedViewDirection;  // Закрыть на изменение направления просмотра (перекаты, отталкивания)
        private Vector2 _viewDirection;
        public Vector2 ViewDirection
        {
            protected set
            {
                if (value != _viewDirection && !_closedViewDirection)
                {
                    _viewDirection = value;
                    OnViewDirectionChanged?.Invoke(value);
                }
            }

            get { return _viewDirection; }
        }

        private bool _closedMoveDirection; // Закрыть на изменение направления движения (перекаты, отталкивания)
        private Vector2 _moveDirection;
        public Vector2 MoveDirection
        {
            protected set
            {
                if (value != _moveDirection && !_closedMoveDirection)
                {
                    _moveDirection = value;
                    OnMoveDirectionChanged?.Invoke(value);
                }
            }

            get { return _moveDirection; }
        }

        protected new Rigidbody2D rigidbody2D;

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

        public void CloseChangeDirections()
        {
            _closedMoveDirection = true;
            _closedViewDirection = true;
        }

        public void OpenChangeDirections()
        {
            _closedMoveDirection = false;
            _closedViewDirection = false;
        }

        public void SetDefaultParameters()
        {
            CurrentSpeed = DefaultSpeed;
            CurrentAcceleration = DefaultAcceleration;
            MoveDirection = Vector2.zero;
            ViewDirection = Vector2.down;
        }

        protected virtual void OnDestroy()
        {
            OnViewDirectionChanged = null;
            OnMoveDirectionChanged = null;
        }
    }
}
