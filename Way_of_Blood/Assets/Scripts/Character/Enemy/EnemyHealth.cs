using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character.Enemy
{
    public class EnemyHealth : CharacterHealth
    {
        protected override void Start()
        {
            base.Start();
            OnHealthChange += OnHealthChangeHandler;
        }

        private void OnHealthChangeHandler(int newHealth)
        {
            if (newHealth == 0)
            {
                CharacterController controller;
                if (TryGetComponent<CharacterController>(out controller))
                    controller.Die();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnHealthChange -= OnHealthChangeHandler;
        }
    }
}
