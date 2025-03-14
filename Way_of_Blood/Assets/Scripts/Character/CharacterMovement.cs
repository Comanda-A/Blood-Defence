using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        /// <summary>
        /// Вызывается каждый раз при изменении вектора направления взгляда (Vector2 ViewDirection).
        /// </summary>
        public event UnityAction<Vector2> OnViewDirectionChanged;

        /// <summary>
        /// Вызывается каждый раз при изменении вектора перемещения (Vector2 MoveDirection).
        /// </summary>
        public event UnityAction<Vector2> OnMoveDirectionChanged;

        /// <summary>
        /// Вызывается при перемещении сущности (MoveDirection != Vector2.Zero).
        /// </summary>
        public event UnityAction<Vector2> OnMoving;

        [Header("Movement parameters")]
        public float DefaultSpeed;              // Скорость по умолчанию
        public float DefaultAcceleration;       // Ускорение по умолчанию
        public float CurrentSpeed;              // Скорость
        public float CurrentAcceleration;       // Ускорение

        private bool _closedViewDirection;              // Закрыть на изменение направления просмотра (перекаты, отталкивания)
        private Vector2 _viewDirection = Vector2.zero;  // ViewDirection

        /// <summary>
        /// Направление взгляда.
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

        private bool _closedMoveDirection;              // Закрыть на изменение направления движения (перекаты, отталкивания)
        private Vector2 _moveDirection = Vector2.zero;  // MoveDirection

        /// <summary>
        /// Направление движения.
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

        // Постоянное перемещение (перекаты, ускорения)
        // Дефолтные параметры при завершении постоянного перемещения
        private bool _defaultOnCompletionForcedMovement;
        private float _forcedMovementTime;           // Время запрета изменений
        private Coroutine _forcedMovementCoroutine;  // Корутина

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
        /// Устанавливает доступ к изменениям MoveDirection. true - изменения доступны, false - изменения закрыты
        /// </summary>
        /// <param name="value"></param>
        public void SetMoveDirectionForChanges(bool value, Vector2 moveDirection)
        {
            MoveDirection = moveDirection;
            _closedMoveDirection = !value;
        }

        /// <summary>
        /// Установить постоянное движение в определенном направлении.
        /// </summary>
        /// <param name="move">Направление перемещения</param>
        /// <param name="view">Направление взгляда</param>
        /// <param name="time">Время перемещения (длительность)</param>
        /// <param name="speed">Скорость</param>
        /// <param name="acceleration">Ускорение</param>
        /// <param name="defaultOnCompletion">Вернуть параметры обратно при завершении</param>
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
