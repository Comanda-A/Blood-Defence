using System.Collections;
using UnityEngine;

namespace WayOfBlood.Character.Player
{
    public class PlayerHealth : CharacterHealth
    {
        [Header("Regeneration")]
        public float RegenerationTime; // Время регенерации 1 hp
        public int CostBloodlust;      // Затраты жажды крови на 1 hp

        private CharacterBloodlust characterBloodlust;
        private Coroutine regenerationCoroutine;

        protected override void Start()
        {
            base.Start();

            characterBloodlust = GetComponent<CharacterBloodlust>();
            if (characterBloodlust != null)
            {
                characterBloodlust.OnBloodlustChange += OnBloodlustChangeHandler;
            }

            OnHealthChange += OnHealthChangeHandler;
        }

        private void OnHealthChangeHandler(int newHealth)
        {
            if (Health == 0)
            {
                GetComponent<PlayerController>().Die();
                return;
            }

            if (Health < MaxHealth && characterBloodlust.Bloodlust >= CostBloodlust)
            {
                // Если здоровье изменилось, перезапускаем корутину регенерации
                if (regenerationCoroutine != null)
                {
                    StopCoroutine(regenerationCoroutine);
                    regenerationCoroutine = null;
                }
                regenerationCoroutine = StartCoroutine(RegenerateHealth());
            }
        }

        private void OnBloodlustChangeHandler(int newBloodlust)
        {
            if (Health < MaxHealth && characterBloodlust.Bloodlust >= CostBloodlust)
            {
                if (regenerationCoroutine == null)
                    regenerationCoroutine = StartCoroutine(RegenerateHealth());
            }
        }

        private IEnumerator RegenerateHealth()
        {
            yield return new WaitForSeconds(RegenerationTime);

            // Проверяем условия для регенерации
            if (Health < MaxHealth && characterBloodlust.Bloodlust >= CostBloodlust)
            {
                // Добавляем 1 HP и тратим жажду крови
                AddHealth(1);
                characterBloodlust.TakeBloodlust(CostBloodlust);
            }

            regenerationCoroutine = null;
            yield return null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Отписываемся от событий при уничтожении объекта
            if (characterBloodlust != null)
            {
                characterBloodlust.OnBloodlustChange -= OnBloodlustChangeHandler;
            }

            OnHealthChange -= OnHealthChangeHandler;

            // Останавливаем корутину, если она активна
            if (regenerationCoroutine != null)
            {
                StopCoroutine(regenerationCoroutine);
                regenerationCoroutine = null;
            }
        }
    }
}