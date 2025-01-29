using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Shooting;

namespace WayOfBlood.Character
{
    public class CharacterShot : MonoBehaviour
    {
        public event UnityAction OnShot;        // Событие выстрела 
        public float ShotCooldown = 0.1f;       // Задержка между выстрелами
        protected float lastShotTime = 0f;      // Время последнего выстрела

        public void Shot(Vector2 position, Vector2 direction)
        {
            Shot(position, direction, 10, 3, 1);
        }

        public void Shot(
            Vector2 position,
            Vector2 direction,
            float speed,
            float lifetime,
            int damage)
        {
            if (Time.time > lastShotTime + ShotCooldown)
            {
                ShootingManager.CreateBullet(position, direction, speed, lifetime, damage);
                OnShot?.Invoke();
                lastShotTime = Time.time;
            }
        }
    }
}