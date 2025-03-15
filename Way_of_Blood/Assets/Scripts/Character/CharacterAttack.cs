using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Character.Player;

namespace WayOfBlood.Character
{
    public class CharacterAttack : MonoBehaviour
    {
        public event UnityAction OnAttack;                      // ������� �����
        public event UnityAction<CharacterHealth> OnDamage;     // ������� ��������� �����

        [Header("Attack parameters")]
        public int AttackDamage;                // ����
        public float AttackCooldown = 0.1f;     // �������� ����� �������

        [Header("Attack collision parameters")]
        public float HittingAngle = 90f; // ���� �����
        public float AttackRadius = 2f;  // ������ �����
        public LayerMask enemyLayer;     // ����, �� ������� ��������� �����

        private CharacterMovement characterMovement;
        private float lastAttackTime = 0f;      // ����� ���������� ��������

        protected virtual void Start()
        {
            characterMovement = GetComponent<CharacterMovement>();
        }

        public void Attack()
        {
            if (Time.time > lastAttackTime + AttackCooldown)
            {
                lastAttackTime = Time.time;
                ProcessAttack();
                OnAttack?.Invoke();
            }
        }

        private void ProcessAttack()
        {
            // �������� ����������� ������� ������
            Vector2 attackDirection = characterMovement.ViewDirection;

            // ������� ���� ������ � ������� �����
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, AttackRadius, enemyLayer);

            // �������� �� ���� ��������� ������
            foreach (var enemy in hitEnemies)
            {
                // ��������� ����������� �� ������ � �����
                Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;

                // ���������, ��������� �� ���� � �������� ���� �����
                if (Vector2.Angle(attackDirection, directionToEnemy) < HittingAngle / 2)
                {
                    // ������� ���� ������� ���������� �����
                    if (enemy.TryGetComponent<CharacterHealth>(out var enemyHealth) &&
                        enemy.TryGetComponent<CharacterController>(out var controller) &&
                        !controller.IsDead)
                    {
                        enemyHealth.TakeDamage(AttackDamage);
                        OnDamage?.Invoke(enemyHealth);
                        break; // ��������� ���� ����� ��������� ����� ������ �����
                    }
                }
            }
        }

        protected virtual void OnDestroy()
        {
            OnAttack = null;
        }

        // ������������ ������� � ���� ����� � ���������
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);

            // ������ �����, ������������ ���� �����
            Vector2 attackDirection = characterMovement != null ? characterMovement.ViewDirection : Vector2.right;
            Vector2 leftBoundary = Quaternion.Euler(0, 0, HittingAngle / 2) * attackDirection * AttackRadius;
            Vector2 rightBoundary = Quaternion.Euler(0, 0, -HittingAngle / 2) * attackDirection * AttackRadius;

            Gizmos.DrawLine(transform.position, (Vector2)transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + rightBoundary);
        }
    }
}
