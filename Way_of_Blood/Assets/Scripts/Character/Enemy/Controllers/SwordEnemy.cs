using UnityEngine;

namespace WayOfBlood.Character.Enemy
{
    public class SwordEnemy : EnemyController
    {
        [Header("Settings")]
        public float AttackRange = 2f;              // ��������� ��� ����� �����

        private CharacterMovement _playerMovement;
        private EnemyAttack _enemyAttack;

        protected override void Start()
        {
            base.Start();
            _playerMovement = _player.GetComponent<CharacterMovement>();
            _enemyAttack = GetComponent<EnemyAttack>();
        }

        private void Update()
        {
            if (_player == null || isDead)
            {
                _enemyMovement.StopMovement();
                return;
            }

            // ��������� ����������� � ���������� ������
            Vector2 directionToPlayer = ((Vector2)(_player.position - transform.position)).normalized;

            // �������� � ���������� ������
            if (Vector2.Distance(transform.position, _player.position) > AttackRange)
            {
                _enemyMovement.MoveToPosition(_player.position);
            }
            else
            {
                _enemyMovement.StopMovement(); // ��������������� ��� �����
            }

            // ����� �����, ���� ������ � ������
            if (Vector2.Distance(transform.position, _player.position) <= AttackRange)
            {
                _enemyAttack.Attack();
            }

            // �������� �� ������������� ��������� �� ����
            CheckForBulletEvasion();

            // �������� �� ������������� ��������, ���� ����� ������� ������
            if (Vector2.Distance(transform.position, _player.position) <= AttackRange * 0.8f)
            {
                PerformRoll(GetEvadeDirection(_playerMovement.MoveDirection));
            }
        }
    }
}