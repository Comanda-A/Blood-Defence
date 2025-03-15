using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Character.Player;

namespace WayOfBlood.Character
{
    public class CharacterAttack : MonoBehaviour
    {
        public event UnityAction OnAttack;                      // Событие атаки
        public event UnityAction<CharacterHealth> OnDamage;     // Событие нанесения урона

        [Header("Attack parameters")]
        public int AttackDamage;                // Урон
        public float AttackCooldown = 0.1f;     // Задержка между атаками

        [Header("Attack collision parameters")]
        public float HittingAngle = 90f; // Угол атаки
        public float AttackRadius = 2f;  // Радиус атаки
        public LayerMask enemyLayer;     // Слой, на котором находятся враги

        private CharacterMovement characterMovement;
        private float lastAttackTime = 0f;      // Время последнего выстрела

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
            // Получаем направление взгляда игрока
            Vector2 attackDirection = characterMovement.ViewDirection;

            // Находим всех врагов в радиусе атаки
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, AttackRadius, enemyLayer);

            // Проходим по всем найденным врагам
            foreach (var enemy in hitEnemies)
            {
                // Вычисляем направление от игрока к врагу
                Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;

                // Проверяем, находится ли враг в пределах угла атаки
                if (Vector2.Angle(attackDirection, directionToEnemy) < HittingAngle / 2)
                {
                    // Наносим урон первому найденному врагу
                    if (enemy.TryGetComponent<CharacterHealth>(out var enemyHealth) &&
                        enemy.TryGetComponent<CharacterController>(out var controller) &&
                        !controller.IsDead)
                    {
                        enemyHealth.TakeDamage(AttackDamage);
                        OnDamage?.Invoke(enemyHealth);
                        break; // Прерываем цикл после нанесения урона одному врагу
                    }
                }
            }
        }

        protected virtual void OnDestroy()
        {
            OnAttack = null;
        }

        // Визуализация радиуса и угла атаки в редакторе
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);

            // Рисуем линии, показывающие угол атаки
            Vector2 attackDirection = characterMovement != null ? characterMovement.ViewDirection : Vector2.right;
            Vector2 leftBoundary = Quaternion.Euler(0, 0, HittingAngle / 2) * attackDirection * AttackRadius;
            Vector2 rightBoundary = Quaternion.Euler(0, 0, -HittingAngle / 2) * attackDirection * AttackRadius;

            Gizmos.DrawLine(transform.position, (Vector2)transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + rightBoundary);
        }
    }
}
