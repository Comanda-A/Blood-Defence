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
            if (Time.time > lastShotTime + ShotCooldown)
            {
                ShootingManager.CreateBullet(position, direction);
                OnShot?.Invoke();
                lastShotTime = Time.time;
            }
        }
    }
}