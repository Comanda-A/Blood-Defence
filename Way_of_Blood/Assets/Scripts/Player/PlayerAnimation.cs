using UnityEngine;

namespace WayOfBlood.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        public PlayerAnimationType AnimationType { get; private set; }

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private PlayerMovement playerMovement;
        private PlayerAttack playerAttack;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            playerAttack = GetComponent<PlayerAttack>();
        }

        private void Update()
        {
            if (playerAttack.Attack)
            {
                SetAnimation(PlayerAnimationType.Attack);
            }
            else
            {
                if (playerMovement.MoveDirection != Vector2.zero)
                    SetAnimation(PlayerAnimationType.Run);
                else
                    SetAnimation(PlayerAnimationType.Idle);
            }

            animator.SetFloat("X", playerMovement.ViewDirection.x);
            animator.SetFloat("Y", playerMovement.ViewDirection.y);
            spriteRenderer.flipX = playerMovement.ViewDirection.x < 0;
        }

        private void SetAnimation(PlayerAnimationType type)
        {
            AnimationType = type;

            animator.SetBool("Run", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Attack", false);

            switch (type)
            {
                case PlayerAnimationType.Idle:
                    animator.SetBool("Idle", true);
                    break;
                case PlayerAnimationType.Run:
                    animator.SetBool("Run", true);
                    break;
                case PlayerAnimationType.Attack:
                    animator.SetBool("Attack", true);
                    break;
            }
        }

        public enum PlayerAnimationType
        {
            Idle,
            Run,
            Attack
        }
    }
}

