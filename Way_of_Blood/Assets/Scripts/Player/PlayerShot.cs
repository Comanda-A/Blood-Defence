using UnityEngine;
using WayOfBlood.Player.Input;

namespace WayOfBlood.Player
{
    public class PlayerShot : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab; // Префаб пули

        private PlayerInputBase playerInputSystem;
        private Camera mainCamera;

        void Start()
        {
            playerInputSystem = GetComponent<PlayerInputBase>();
            mainCamera = Camera.main; // Получаем основную камеру
        }

        void Update()
        {
            // Проверяем, нажата ли клавиша выстрела
            if (playerInputSystem.ShotKeyPressed)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            // Получаем позицию курсора в мировых координатах
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            mousePosition.z = 0; // Обнуляем Z-координату, так как мы работаем в 2D

            // Вычисляем направление от игрока к курсору
            Vector2 direction = (mousePosition - transform.position).normalized;

            // Создаем пулю
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Задаем направление пули
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