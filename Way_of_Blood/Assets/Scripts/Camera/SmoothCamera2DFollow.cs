using UnityEngine;

public class SmoothCamera2DFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Цель, за которой следует камера (игрок)

    [Header("Camera Settings")]
    public Vector2 offset = new Vector2(0, 0); // Смещение камеры относительно цели
    public float smoothSpeed = 0.125f; // Скорость плавного перемещения камеры

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not set for SmoothCamera2DFollow!");
            return;
        }

        // Вычисляем желаемую позицию камеры
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // Плавно интерполируем текущую позицию камеры к желаемой
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Применяем новую позицию камеры
        transform.position = smoothedPosition;
    }
}