using System.Collections;
using UnityEngine;

namespace WayOfBlood.Character.Player
{
    public class PlayerHealth : CharacterHealth
    {
        [Header("Regeneration")]
        public float RegenerationTime; // ����� ����������� 1 hp
        public int CostBloodlust;      // ������� ����� ����� �� 1 hp

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
                // ���� �������� ����������, ������������� �������� �����������
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

            // ��������� ������� ��� �����������
            if (Health < MaxHealth && characterBloodlust.Bloodlust >= CostBloodlust)
            {
                // ��������� 1 HP � ������ ����� �����
                AddHealth(1);
                characterBloodlust.TakeBloodlust(CostBloodlust);
            }

            regenerationCoroutine = null;
            yield return null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // ������������ �� ������� ��� ����������� �������
            if (characterBloodlust != null)
            {
                characterBloodlust.OnBloodlustChange -= OnBloodlustChangeHandler;
            }

            OnHealthChange -= OnHealthChangeHandler;

            // ������������� ��������, ���� ��� �������
            if (regenerationCoroutine != null)
            {
                StopCoroutine(regenerationCoroutine);
                regenerationCoroutine = null;
            }
        }
    }
}