using UnityEngine;

namespace WayOfBlood.Character.Enemy
{
    public class EnemyController : CharacterController
    {
        [Header("Settings")]
        public float AttackRange = 2f;              // ��������� ��� ����� �����
        public float ShootRange = 5f;               // ��������� ��� ��������
        public float RollCooldown = 2f;             // ����� ����������� ��������
        public float BulletEvadeDistance = 1.5f;    // ��������� ��� ��������� �� ����
        public LayerMask BulletMask;

        private Transform player;                   // ��������� �����
        private CharacterMovement playerMovement;
        private EnemyMovement enemyMovement;
        private EnemyShot enemyShot;
        private EnemyAttack enemyAttack;
        private EnemyRoll enemyRoll;
        private float lastRollTime;                 // ����� ���������� ��������

        protected override void Start()
        {
            base.Start();

            // ������� ������ �� �����
            player = GameObject.FindGameObjectWithTag("Player").transform;

            // �������� ����������
            enemyMovement = GetComponent<EnemyMovement>();
            enemyShot = GetComponent<EnemyShot>();
            enemyAttack = GetComponent<EnemyAttack>();
            enemyRoll = GetComponent<EnemyRoll>();

            if (player == null)
            {
                Debug.LogError("No players found!");
            }

            lastRollTime = -RollCooldown; // ������������� ������� ���������� ��������
        }

        private void Update()
        {
            if (player == null || isDead)
            {
                enemyMovement.StopMovement();
                return;
            }

            // ��������� ����������� � ���������� ������
            Vector2 directionToPlayer = ((Vector2)(player.position - transform.position)).normalized;

            // �������� � ���������� ������
            if (Vector2.Distance(transform.position, player.position) > AttackRange)
            {
                enemyMovement.MoveToPosition(player.position);
            }
            else
            {
                enemyMovement.StopMovement(); // ��������������� ��� �����
            }

            // ����� �����, ���� ������ � ������
            if (Vector2.Distance(transform.position, player.position) <= AttackRange)
            {
                enemyAttack.Attack();
            }
            // ��������, ���� ����� � ���� ���������
            else if (Vector2.Distance(transform.position, player.position) <= ShootRange)
            {
                enemyShot.Shot(transform.position, directionToPlayer);
            }

            // �������� �� ������������� ��������� �� ����
            CheckForBulletEvasion();

            // �������� �� ������������� ��������, ���� ����� ������� ������
            if (Vector2.Distance(transform.position, player.position) <= AttackRange * 0.8f && Time.time - lastRollTime >= RollCooldown)
            {
                PerformRoll(GetEvadeDirection(playerMovement.MoveDirection));
            }
        }

        private void CheckForBulletEvasion()
        {
            // ���� ��� ���������� �� ���� ���� � ������� bulletEvadeDistance
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, BulletEvadeDistance, BulletMask);

            // ��������� ������ ��������� ���������
            foreach (var hit in hits)
            {
                // �������� Rigidbody2D ����
                Rigidbody2D bulletRigidbody = hit.GetComponent<Rigidbody2D>();
                if (bulletRigidbody == null) continue;

                // ����������� �� ���� � �����
                Vector2 directionToEnemy = (transform.position - hit.transform.position).normalized;

                // ����������� �������� ����
                Vector2 bulletVelocity = bulletRigidbody.linearVelocity.normalized;

                // ���� ����� ������������ �������� ���� � ������������ �� �����
                float angle = Vector2.Angle(bulletVelocity, directionToEnemy);

                // ���� ���� ����� ����� � ����� (���� ������ � 0), ��������� �������
                if (angle < 30f) // 30 �������� - ���������� ����
                {
                    // ��������� ���������������� ����������� ��� �������
                    Vector2 evadeDirection = GetEvadeDirection(bulletVelocity);
                    PerformRoll(evadeDirection);
                    break; // ���������� ������ �� ����� ���� �� ���
                }
            }
        }

        // ����� ��� ���������� ����������� �������
        private Vector2 GetEvadeDirection(Vector2 bulletDirection)
        {
            // ���������������� ����������� (����� ��� ������)
            Vector2 perpendicular = new Vector2(-bulletDirection.y, bulletDirection.x);

            // �������� ��������� ����������� (����� ��� ������)
            if (Random.value > 0.5f)
            {
                perpendicular = -perpendicular; // ������ ����������� �� ���������������
            }

            return perpendicular.normalized;
        }

        // ����������� ����� PerformRoll � ������ �����������
        private void PerformRoll(Vector2 direction)
        {
            // �������� ����������� � ����� Roll (������������, ��� enemyRoll.Roll ��������� �����������)
            enemyMovement.MoveToPosition((Vector2)transform.position + direction);
            enemyRoll.Roll();
            lastRollTime = Time.time; // ��������� ����� ���������� ��������
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