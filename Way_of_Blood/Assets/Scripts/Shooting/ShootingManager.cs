using System.Collections.Generic;
using UnityEngine;


namespace WayOfBlood.Shooting
{
    public class ShootingManager : MonoBehaviour
    {
        private static ShootingManager instance;

        [Header("Bullet Settings")]
        [SerializeField] private GameObject bulletPrefab;   // ������ ����
        [SerializeField] private float bulletSpeed = 10f;   // �������� ����
        [SerializeField] private float bulletLifetime = 3f; // ����� ����� ����
        [SerializeField] private int bulletDamage = 1;     // ���� ����

        private Camera mainCamera;
        private List<GameObject> bullets;

        private void Start()
        {
            instance = this;
            mainCamera = Camera.main; // �������� �������� ������
            bullets = new List<GameObject>();
        }

        public static GameObject[] GetAllBullets()
        {
            return instance.bullets.ToArray();
        }

        public static GameObject CreateBullet(
            Vector2 position,
            Vector2 direction,
            float speed,
            float lifetime,
            int damage)
        {
            if (instance.bulletPrefab == null)
            {
                Debug.LogError("Bullet prefab is not assigned.");
                return null;
            }

            // ������� ���� �� �������
            GameObject bulletObject = Instantiate(
                instance.bulletPrefab,
                new Vector3(position.x, position.y, instance.bulletPrefab.transform.position.z),
                Quaternion.identity
            );
            var bulletHandler = bulletObject.GetComponent<BulletHandler>();
            bulletHandler.Initialize(speed, lifetime, damage, direction);
            bulletHandler.OnDestroyEvent += OnDestroyBullet;

            instance.bullets.Add(bulletObject);
            

            return bulletObject;
        }

        public static GameObject CreateBullet(
            Vector2 position,
            Vector2 direction) 
        {
            return CreateBullet(
                position,
                direction,
                instance.bulletSpeed,
                instance.bulletLifetime,
                instance.bulletDamage
            );
        }

        private static void OnDestroyBullet(GameObject bullet)
        {
            instance.bullets.Remove(bullet);
        }
    }
}


