using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WayOfBlood.Character;
using WayOfBlood.Character.Player;

namespace WayOfBlood.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private GameObject hpPrefab;
        [SerializeField] private GameObject shieldPrefab;
        [SerializeField] private Transform uiContainer; // Контейнер для HP и щитов

        [SerializeField] private Vector2 startPosition = new Vector2(50, 50);
        [SerializeField] private Vector2 offset = new Vector2(50, 0);

        private List<Image> hpIcons = new List<Image>();
        private List<Image> shieldIcons = new List<Image>();

        private PlayerHealth _playerHealth;
        private PlayerBlood _playerBlood;

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                _playerHealth = player.GetComponent<PlayerHealth>();
                _playerBlood = player.GetComponent<PlayerBlood>();

                if (_playerHealth != null && _playerBlood != null)
                {
                    _playerHealth.OnHealthChange += OnHealthChange;
                    _playerBlood.OnBloodChange += OnBloodChange;

                    InitUI();
                }
                else
                {
                    Debug.LogError("PlayerHealth или PlayerBlood не найдены!");
                }
            }
            else
            {
                Debug.LogError("Игрок не найден!");
            }
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
            {
                _playerHealth.OnHealthChange -= OnHealthChange;
                _playerBlood.OnBloodChange -= OnBloodChange;
            }
        }

        private void InitUI()
        {
            // Создаем иконки здоровья
            for (int i = 0; i < _playerHealth.MaxHealth; i++)
            {
                GameObject hpObj = Instantiate(hpPrefab, uiContainer);
                hpObj.GetComponent<RectTransform>().anchoredPosition = startPosition + i * offset;
                hpIcons.Add(hpObj.GetComponent<Image>());
            }

            // Создаем иконки щитов
            for (int i = 0; i < _playerHealth.ShieldsCount; i++)
            {
                GameObject shieldObj = Instantiate(shieldPrefab, uiContainer);
                shieldObj.GetComponent<RectTransform>().anchoredPosition = startPosition + (_playerHealth.MaxHealth + i) * offset;
                shieldObj.SetActive(false);
                shieldIcons.Add(shieldObj.GetComponent<Image>());
            }

            UpdateHealthUI();
            UpdateShieldUI();
        }

        private void OnHealthChange(int newValue)
        {
            UpdateHealthUI();
            UpdateShieldUI();
        }

        private void OnBloodChange(int newValue)
        {
            UpdateShieldUI();
        }

        private void UpdateHealthUI()
        {
            for (int i = 0; i < hpIcons.Count; i++)
            {
                hpIcons[i].enabled = i < _playerHealth.Health;
            }
        }

        private void UpdateShieldUI()
        {
            int shieldsActive = _playerHealth.ShieldsCount;
            int availableBlood = _playerBlood.Blood;
            int shieldCost = _playerHealth.ShieldCost;

            for (int i = 0; i < shieldIcons.Count; i++)
            {
                if (i < shieldsActive)
                {
                    shieldIcons[i].gameObject.SetActive(true);

                    float fillAmount = 1f;
                    if (availableBlood < shieldCost)
                    {
                        fillAmount = (float)availableBlood / shieldCost;
                    }

                    shieldIcons[i].fillAmount = fillAmount;
                    availableBlood -= shieldCost;
                }
                else
                {
                    shieldIcons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
