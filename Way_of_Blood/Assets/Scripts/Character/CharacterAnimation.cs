using UnityEngine;

namespace WayOfBlood.Character
{
    public class CharacterAnimation : MonoBehaviour
    {
        public CharacterAnimationType AnimationType { get; private set; }

        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private CharacterMovement ñharacterMovement;

        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            ñharacterMovement = GetComponent<CharacterMovement>();
            GetComponent<CharacterAttack>().OnAttack += PlayAttackAnimation;
            GetComponent<CharacterController>().OnDeath += StartDeathAnimation;
        }

        private void PlayAttackAnimation()
        {
            SetAnimation(CharacterAnimationType.Attack);
        }

        protected virtual void Update()
        {
            if (ñharacterMovement.MoveDirection != Vector2.zero)
                SetAnimation(CharacterAnimationType.Run);
            else
                SetAnimation(CharacterAnimationType.Idle);
            
            animator.SetFloat("X", ñharacterMovement.ViewDirection.x);
            animator.SetFloat("Y", ñharacterMovement.ViewDirection.y);
            spriteRenderer.flipX = ñharacterMovement.ViewDirection.x < 0;
        }

        private void StartDeathAnimation()
        {
            SetAnimation(CharacterAnimationType.Death);
        }

        private void SetAnimation(CharacterAnimationType type)
        {
            AnimationType = type;

            animator.SetBool("Run", false);
            animator.SetBool("Idle", false);

            switch (type)
            {
                case CharacterAnimationType.Idle:
                    animator.SetBool("Idle", true);
                    break;
                case CharacterAnimationType.Run:
                    animator.SetBool("Run", true);
                    break;
                case CharacterAnimationType.Attack:
                    animator.SetTrigger("Attack");
                    break;
                case CharacterAnimationType.Death:
                    animator.SetTrigger("Death");
                    break;
            }
        }

        private void OnDestroy()
        {
            GetComponent<CharacterAttack>().OnAttack -= PlayAttackAnimation;
            GetComponent<CharacterController>().OnDeath -= StartDeathAnimation;
        }

        public enum CharacterAnimationType
        {
            Idle,
            Run,
            Attack,
            Death
        }
    }
}