using System.Collections.Generic;
using UnityEngine;
using WayOfBlood.Character;

namespace WayOfBlood.UI
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject HpPrefab; // Префаб иконки здоровья
        public Vector3 HpOffset;    // Смещение между иконками

        private CharacterHealth _playerHealth; // Ссылка на здоровье игрока
        private List<GameObject> _hpIcons;

        private void Start()
        {
            _hpIcons = new List<GameObject>();

            // Находим игрока по тегу
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && HpPrefab != null)
            {
                // Получаем компонент CharacterHealth
                _playerHealth = player.GetComponent<CharacterHealth>();
                if (_playerHealth != null)
                {
                    // Подписываемся на события изменения здоровья
                    _playerHealth.OnHealthChange += OnHealthChange;
                    _playerHealth.OnMaxHealthChange += OnMaxHealthChange;

                    // Инициализируем UI при старте
                    OnMaxHealthChange(_playerHealth.MaxHealth);
                    OnHealthChange(_playerHealth.Health);
                }
                else
                {
                    Debug.LogError("CharacterHealth component not found on player!");
                }
            }
            else
            {
                Debug.LogError("Player or HpPrefab is not assigned!");
            }
        }

        private void OnDestroy()
        {
            // Отписываемся от событий при уничтожении объекта
            if (_playerHealth != null)
            {
                _playerHealth.OnHealthChange -= OnHealthChange;
                _playerHealth.OnMaxHealthChange -= OnMaxHealthChange;
            }
        }

        private void OnHealthChange(int newValue)
        {
            // Активируем или деактивируем дочерние объекты в зависимости от текущего здоровья
            for (int i = 0; i < _hpIcons.Count; i++)
            {
                _hpIcons[i].SetActive(i < newValue);
            }
        }

        private void OnMaxHealthChange(int newValue)
        {
            // Удаляем все дочерние объекты
            foreach (GameObject go in _hpIcons)
            {
                Destroy(go);
            }

            // Создаем новые иконки здоровья
            for (int i = 0; i < newValue; i++)
            {
                var go = Instantiate(HpPrefab, transform);
                go.transform.localPosition = HpOffset * i;
                _hpIcons.Add(go);
            }

            // Обновляем текущее состояние здоровья
            OnHealthChange(_playerHealth.Health);
        }
    }
}