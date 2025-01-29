using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Input;
using WayOfBlood.Shooting;

namespace WayOfBlood.Character.Enemy
{
    public class EnemyShot : CharacterShot
    {
        public float ShotRadius = 1f;

        public new void Shot(Vector2 position, Vector2 direction)
        {
            base.Shot((Vector2)transform.position + direction * ShotRadius, direction);
        }
    }
}