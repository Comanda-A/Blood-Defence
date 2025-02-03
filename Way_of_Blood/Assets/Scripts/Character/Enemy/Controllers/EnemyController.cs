using UnityEngine;

namespace WayOfBlood.Character.Enemy
{
    public class EnemyController : CharacterController
    {
        [Header("Settings")]
        public float RollCooldown = 2f;             // ����� ����������� ��������
        public float BulletEvadeDistance = 1.5f;    // ��������� ��� ��������� �� ����
        public LayerMask BulletMask;

        protected Transform _player;                // ��������� �����
        protected Transform _transform;
        protected EnemyMovement _enemyMovement;
        protected EnemyRoll _enemyRoll;

        private float _lastRollTime;                 // ����� ���������� ��������

        protected override void Start()
        {
            base.Start();

            _transform = transform;

            // ������� ������ �� �����
            _player = GameObject.FindGameObjectWithTag("Player").transform;

            if (_player == null)
                Debug.LogError("No players found!");

            // �������� ����������
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyRoll = GetComponent<EnemyRoll>();
            
            _lastRollTime = -RollCooldown; // ������������� ������� ���������� ��������
        }

        protected void CheckForBulletEvasion()
        {
            // ���� ��� ���������� �� ���� ���� � ������� bulletEvadeDistance
            Collider2D[] hits = Physics2D.OverlapCircleAll(_transform.position, BulletEvadeDistance, BulletMask);

            // ��������� ������ ��������� ���������
            foreach (var hit in hits)
            {
                // �������� Rigidbody2D ����
                Rigidbody2D bulletRigidbody = hit.GetComponent<Rigidbody2D>();
                if (bulletRigidbody == null) continue;

                // ����������� �� ���� � �����
                Vector2 directionToEnemy = (_transform.position - hit.transform.position).normalized;

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
        protected Vector2 GetEvadeDirection(Vector2 bulletDirection)
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
        protected void PerformRoll(Vector2 direction)
        {
            // �������� ����������� � ����� Roll (������������, ��� enemyRoll.Roll ��������� �����������)
            _enemyMovement.MoveToPosition((Vector2)_transform.position + direction);
            _enemyRoll.Roll();
            _lastRollTime = Time.time; // ��������� ����� ���������� ��������
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