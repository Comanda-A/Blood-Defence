using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WayOfBlood.Character;

namespace WayOfBlood.UI
{
    public class HealthUI : MonoBehaviour
    {
        public Image HP;
        public Image[] S;

        private CharacterHealth _playerHealth; // Ссылка на здоровье игрока
        private CharacterBlood _playerBlood;

        private void Start()
        {
            // Находим игрока по тегу
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Получаем компонент CharacterHealth
                _playerHealth = player.GetComponent<CharacterHealth>();
                _playerBlood = player.GetComponent<CharacterBlood>();
                if (_playerHealth != null && _playerBlood != null)
                {
                    // Подписываемся на события изменения здоровья
                    _playerHealth.OnHealthChange += OnHealthChange;
                    _playerBlood.OnBloodChange += OnBloodChange;
                }
                else
                {
                    Debug.LogError("CharacterHealth or CharacterBlood component not found on player!");
                }
            }
            else
            {
                Debug.LogError("Player is not assigned!");
            }
        }

        private void OnDestroy()
        {
            // Отписываемся от событий при уничтожении объекта
            if (_playerHealth != null)
            {
                _playerHealth.OnHealthChange -= OnHealthChange;
                _playerBlood.OnBloodChange -= OnBloodChange;
            }
        }

        private void OnBloodChange(int newValue)
        {
            for (int i = 0; i < S.Length; i++)
            {
                S[i].fillAmount = 0;
            }

            if (newValue == 0)
                return;

            for (int i = 1; i <= S.Length; i++)
            {
                if (i * 2 < newValue)
                {
                    S[i - 1].fillAmount = 1;
                }
                else if (i * 2 == newValue)
                {
                    S[i - 1].fillAmount = 1;
                    break;
                }
                else
                {
                    S[i - 1].fillAmount = 0.5f;
                    break;
                }
            }
        }

        private void OnHealthChange(int newValue)
        {
            if (newValue == 0)
            {
                HP.gameObject.SetActive(false);
            }
            else
            {
                HP.gameObject.SetActive(true);
            }
        }
    }
}