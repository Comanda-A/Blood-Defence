using UnityEngine;
using WayOfBlood.Player.Input;

namespace WayOfBlood.Player
{
    public class PlayerShot : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab; // ������ ����

        private PlayerInputBase playerInputSystem;
        private Camera mainCamera;

        void Start()
        {
            playerInputSystem = GetComponent<PlayerInputBase>();
            mainCamera = Camera.main; // �������� �������� ������
        }

        void Update()
        {
            // ���������, ������ �� ������� ��������
            if (playerInputSystem.ShotKeyPressed)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            // �������� ������� ������� � ������� �����������
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            mousePosition.z = 0; // �������� Z-����������, ��� ��� �� �������� � 2D

            // ��������� ����������� �� ������ � �������
            Vector2 direction = (mousePosition - transform.position).normalized;

            // ������� ����
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // ������ ����������� ����
            Bullet bulletComponent = newBullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.SetDirection(direction);
            }
            else
            {
                Debug.LogError("Bullet prefab is missing Bullet component.");
            }
        }
    }
}