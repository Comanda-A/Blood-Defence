using UnityEngine;

namespace WayOfBlood.Character.Enemy
{
    public class EnemyController : CharacterController
    {
        [Header("Settings")]
        public float AttackRange = 2f;              // Дистанция для атаки мечом
        public float ShootRange = 5f;               // Дистанция для стрельбы
        public float RollCooldown = 2f;             // Время перезарядки переката
        public float BulletEvadeDistance = 1.5f;    // Дистанция для уклонения от пуль
        public LayerMask BulletMask;

        private Transform player;                   // Ближайший игрок
        private CharacterMovement playerMovement;
        private EnemyMovement enemyMovement;
        private EnemyShot enemyShot;
        private EnemyAttack enemyAttack;
        private EnemyRoll enemyRoll;
        private float lastRollTime;                 // Время последнего переката

        protected override void Start()
        {
            base.Start();

            // Находим игрока на сцене
            player = GameObject.FindGameObjectWithTag("Player").transform;

            // Получаем компоненты
            enemyMovement = GetComponent<EnemyMovement>();
            enemyShot = GetComponent<EnemyShot>();
            enemyAttack = GetComponent<EnemyAttack>();
            enemyRoll = GetComponent<EnemyRoll>();

            if (player == null)
            {
                Debug.LogError("No players found!");
            }

            lastRollTime = -RollCooldown; // Инициализация времени последнего переката
        }

        private void Update()
        {
            if (player == null || isDead)
            {
                enemyMovement.StopMovement();
                return;
            }

            // Вычисляем направление к ближайшему игроку
            Vector2 directionToPlayer = ((Vector2)(player.position - transform.position)).normalized;

            // Движение к ближайшему игроку
            if (Vector2.Distance(transform.position, player.position) > AttackRange)
            {
                enemyMovement.MoveToPosition(player.position);
            }
            else
            {
                enemyMovement.StopMovement(); // Останавливаемся для атаки
            }

            // Атака мечом, если близко к игроку
            if (Vector2.Distance(transform.position, player.position) <= AttackRange)
            {
                enemyAttack.Attack();
            }
            // Стрельба, если игрок в зоне видимости
            else if (Vector2.Distance(transform.position, player.position) <= ShootRange)
            {
                enemyShot.Shot(transform.position, directionToPlayer);
            }

            // Проверка на необходимость уклонения от пуль
            CheckForBulletEvasion();

            // Проверка на необходимость переката, если игрок слишком близко
            if (Vector2.Distance(transform.position, player.position) <= AttackRange * 0.8f && Time.time - lastRollTime >= RollCooldown)
            {
                PerformRoll(GetEvadeDirection(playerMovement.MoveDirection));
            }
        }

        private void CheckForBulletEvasion()
        {
            // Ищем все коллайдеры на слое пуль в радиусе bulletEvadeDistance
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, BulletEvadeDistance, BulletMask);

            // Проверяем каждый найденный коллайдер
            foreach (var hit in hits)
            {
                // Получаем Rigidbody2D пули
                Rigidbody2D bulletRigidbody = hit.GetComponent<Rigidbody2D>();
                if (bulletRigidbody == null) continue;

                // Направление от пули к врагу
                Vector2 directionToEnemy = (transform.position - hit.transform.position).normalized;

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
        private Vector2 GetEvadeDirection(Vector2 bulletDirection)
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
        private void PerformRoll(Vector2 direction)
        {
            // Передаем направление в метод Roll (предполагаем, что enemyRoll.Roll принимает направление)
            enemyMovement.MoveToPosition((Vector2)transform.position + direction);
            enemyRoll.Roll();
            lastRollTime = Time.time; // Обновляем время последнего переката
        }

        public override void Die()
        {
            base.Die();

            transform.position += new Vector3(0, 0, 1);

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 3);
        }
    }
}