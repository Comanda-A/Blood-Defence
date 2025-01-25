using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f; // �������� ����
    public float lifetime = 3f; // ����� ����� ����
    public float damage = 10f; // ���� ����

    private Rigidbody2D rb;
    private Vector2 direction; // ����������� �������� ����

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // ���������� ���� ����� �������� �����
    }

    private void FixedUpdate()
    {
        // ������� ���� � �������� �����������
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    // ����� ��� ������� ����������� ����
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;

        // ������������ ���� � ����������� �������� (�����������)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}