using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Input;
using WayOfBlood.Shooting;

namespace WayOfBlood.Character.Player
{
    public class PlayerShot : CharacterShot
    {
        public float ShotRadius = 1f;

        private new Transform transform;
        private InputBase playerInputSystem;
        private Camera mainCamera;

        void Start()
        {
            transform = GetComponent<Transform>();
            playerInputSystem = GetComponent<InputBase>();
            mainCamera = Camera.main; // �������� �������� ������
        }

        void Update()
        {
            // ���������, ������ �� ������� �������� � ������ �� ��������
            if (playerInputSystem.ShotKeyPressed && Time.time > lastShotTime + ShotCooldown)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            // �������� ������� ������� � ������� �����������
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

            // ��������� ����������� �� ������ � �������
            Vector2 direction = ((Vector2)(mousePosition - transform.position)).normalized;
            print(direction.magnitude);

            Shot((Vector2)transform.position + direction * ShotRadius, direction);
        }
    }
}