using UnityEngine;

public class SmoothCamera2DFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // ����, �� ������� ������� ������ (�����)

    [Header("Camera Settings")]
    public Vector2 offset = new Vector2(0, 0); // �������� ������ ������������ ����
    public float smoothSpeed = 0.125f; // �������� �������� ����������� ������

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not set for SmoothCamera2DFollow!");
            return;
        }

        // ��������� �������� ������� ������
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // ������ ������������� ������� ������� ������ � ��������
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ��������� ����� ������� ������
        transform.position = smoothedPosition;
    }
}