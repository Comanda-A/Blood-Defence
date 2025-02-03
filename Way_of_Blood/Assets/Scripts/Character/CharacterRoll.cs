using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character
{
    public class CharacterRoll : MonoBehaviour
    {
        public event UnityAction OnRoll;

        [Header("Roll parameters")]
        public float RollCooldown;  // Кулдауд
        public float RollSpeed;     // Скорость переката
        public float RollDuration;  // Продолжительность переката

        private CharacterMovement characterMovement;
        private float lastRollTime = 0f;


        protected virtual void Start()
        {
            characterMovement = GetComponent<CharacterMovement>();
        }

        public void Roll()
        {
            if (Time.time > lastRollTime + RollCooldown)
            {
                ProcessRoll();
                OnRoll?.Invoke();
                lastRollTime = Time.time;
            }
        }

        protected virtual void ProcessRoll()
        {
            characterMovement.SetConstantDirection(
                characterMovement.MoveDirection,
                characterMovement.ViewDirection,
                RollDuration,
                RollSpeed,
                characterMovement.CurrentAcceleration,
                true
            );
        }

        protected virtual void OnDestroy()
        {
            OnRoll = null;
        }
    }
}