using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f; // Скорость пули
    public float lifetime = 3f; // Время жизни пули
    public float damage = 10f; // Урон пули

    private Rigidbody2D rb;
    private Vector2 direction; // Направление движения пули

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // Уничтожаем пулю через заданное время
    }

    private void FixedUpdate()
    {
        // Двигаем пулю в заданном направлении
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    // Метод для задания направления пули
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;

        // Поворачиваем пулю в направлении движения (опционально)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}