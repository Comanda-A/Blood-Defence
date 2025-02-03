using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Shooting;

namespace WayOfBlood.Character
{
    public class CharacterShot : MonoBehaviour
    {
        public event UnityAction OnShot;        // ������� �������� 
        public float ShotCooldown = 0.1f;       // �������� ����� ����������
        protected float lastShotTime = 0f;      // ����� ���������� ��������

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