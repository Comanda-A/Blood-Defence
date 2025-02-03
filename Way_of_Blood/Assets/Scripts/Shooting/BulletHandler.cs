using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Character;

namespace WayOfBlood.Shooting
{
    public class BulletHandler : MonoBehaviour
    {
        public event UnityAction<GameObject> OnDestroyEvent;

        [Header("Bullet parameters")]
        public float Speed;                 // ��������
        public int Damage;                  // ����

        private new Rigidbody2D rigidbody;
        private Vector2 direction;          // ����������� �������� ����
        private bool isDestroyed;

        public void Initialize(float speed, float lifetime, int damage, Vector2 direction)
        {
            Speed = speed;
            Damage = damage;
            SetDirection(direction);
            Destroy(gameObject, lifetime);
        }

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            isDestroyed = false;
        }

        private void FixedUpdate()
        {
            rigidbody.linearVelocity = direction * Speed;
        }

        public void SetDirection(Vector2 newDirection)
        {
            direction = newDirection;

            // ������������ ���� � ����������� �������� (�����������)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isDestroyed) return;

            CharacterHealth character;
            if (collision.gameObject.TryGetComponent<CharacterHealth>(out character))
            {
                character.TakeDamage(Damage);
            }

            Destroy();
        }

        public void Destroy()
        {
            isDestroyed = true;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            isDestroyed = true;
            OnDestroyEvent?.Invoke(gameObject);
        }
    }
}