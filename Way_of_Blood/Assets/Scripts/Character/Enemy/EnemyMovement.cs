using TMPro;
using UnityEngine;
using UnityEngine.Events;
using WayOfBlood.Input;

namespace WayOfBlood.Character.Enemy
{
    public class EnemyMovement : CharacterMovement
    {
        private new Transform transform;

        protected override void Start()
        {
            base.Start();
            transform = GetComponent<Transform>();
        }

        public void MoveToPosition(Vector2 target)
        {
            MoveDirection = (target - (Vector2)transform.position).normalized;
            ViewDirection = MoveDirection;
        }

        public void StopMovement()
        {  
            MoveDirection = Vector2.zero;
        }    
    }
}