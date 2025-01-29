using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Character;

namespace WayOfBlood.Shooting
{
    public class BulletHandler : MonoBehaviour
    {
        public event UnityAction<GameObject> OnDestroyEvent; 
        
        public float Speed;                 // Скорость
        public int Damage;                  // Урон

        private new Rigidbody2D rigidbody;
        private Vector2 direction;          // Направление движения пули

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
        }

        private void FixedUpdate()
        {
            rigidbody.linearVelocity = direction * Speed;
        }

        public void SetDirection(Vector2 newDirection)
        {
            direction = newDirection;

            // Поворачиваем пулю в направлении движения (опционально)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CharacterHealth character;
            if (collision.gameObject.TryGetComponent<CharacterHealth>(out character))
            {
                character.TakeDamage(Damage);
            }
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke(gameObject);
        }
    }
}