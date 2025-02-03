using UnityEngine;

namespace WayOfBlood.Character.Enemy
{
    public class EnemyController : CharacterController
    {
        [Header("Settings")]
        public float RollCooldown = 2f;             // Время перезарядки переката
        public float BulletEvadeDistance = 1.5f;    // Дистанция для уклонения от пуль
        public LayerMask BulletMask;

        protected Transform _player;                // Ближайший игрок
        protected Transform _transform;
        protected EnemyMovement _enemyMovement;
        protected EnemyRoll _enemyRoll;

        private float _lastRollTime;                 // Время последнего переката

        protected override void Start()
        {
            base.Start();

            _transform = transform;

            // Находим игрока на сцене
            _player = GameObject.FindGameObjectWithTag("Player").transform;

            if (_player == null)
                Debug.LogError("No players found!");

            // Получаем компоненты
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyRoll = GetComponent<EnemyRoll>();
            
            _lastRollTime = -RollCooldown; // Инициализация времени последнего переката
        }

        protected void CheckForBulletEvasion()
        {
            // Ищем все коллайдеры на слое пуль в радиусе bulletEvadeDistance
            Collider2D[] hits = Physics2D.OverlapCircleAll(_transform.position, BulletEvadeDistance, BulletMask);

            // Проверяем каждый найденный коллайдер
            foreach (var hit in hits)
            {
                // Получаем Rigidbody2D пули
                Rigidbody2D bulletRigidbody = hit.GetComponent<Rigidbody2D>();
                if (bulletRigidbody == null) continue;

                // Направление от пули к врагу
                Vector2 directionToEnemy = (_transform.position - hit.transform.position).normalized;

                // Направление движения пули
                Vector2 bulletVelocity = bulletRigidbody.linearVelocity.normalized;

                // Угол между направлением движения пули и направлением на врага
                float angle = Vector2.Angle(bulletVelocity, directionToEnemy);

                // Если пуля летит прямо в врага (угол близок к 0), выполняем перекат
                if (angle < 30f) // 30 градусов - допустимый угол
                {
                    // Вычисляем перпендикулярное направление для уворота
                    Vector2 evadeDirection = GetEvadeDirection(bulletVelocity);
                    PerformRoll(evadeDirection);
                    break; // Уклоняемся только от одной пули за раз
                }
            }
        }

        // Метод для вычисления направления уворота
        protected Vector2 GetEvadeDirection(Vector2 bulletDirection)
        {
            // Перпендикулярное направление (влево или вправо)
            Vector2 perpendicular = new Vector2(-bulletDirection.y, bulletDirection.x);

            // Выбираем случайное направление (влево или вправо)
            if (Random.value > 0.5f)
            {
                perpendicular = -perpendicular; // Меняем направление на противоположное
            }

            return perpendicular.normalized;
        }

        // Обновленный метод PerformRoll с учетом направления
        protected void PerformRoll(Vector2 direction)
        {
            // Передаем направление в метод Roll (предполагаем, что enemyRoll.Roll принимает направление)
            _enemyMovement.MoveToPosition((Vector2)_transform.position + direction);
            _enemyRoll.Roll();
            _lastRollTime = Time.time; // Обновляем время последнего переката
        }

        public override void Die()
        {
            base.Die();

            _transform.position += new Vector3(0, 0, 1);

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 3);
        }
    }
}