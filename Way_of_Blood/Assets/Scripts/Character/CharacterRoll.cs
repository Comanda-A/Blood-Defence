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
            StartCoroutine(RollAction());
        }

        private IEnumerator RollAction()
        {
            characterMovement.CloseChangeDirections();
            characterMovement.CurrentSpeed = RollSpeed;
            yield return new WaitForSeconds(RollDuration);
            characterMovement.OpenChangeDirections();
            characterMovement.SetDefaultParameters();
            yield return null;
        }

        protected virtual void OnDestroy()
        {
            OnRoll = null;
        }
    }
}