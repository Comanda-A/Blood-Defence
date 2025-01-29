using System.Collections.Generic;
using UnityEngine;
using WayOfBlood.Character;

namespace WayOfBlood.UI
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject HpPrefab; // ������ ������ ��������
        public Vector3 HpOffset;    // �������� ����� ��������

        private CharacterHealth _playerHealth; // ������ �� �������� ������
        private List<GameObject> _hpIcons;

        private void Start()
        {
            _hpIcons = new List<GameObject>();

            // ������� ������ �� ����
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && HpPrefab != null)
            {
                // �������� ��������� CharacterHealth
                _playerHealth = player.GetComponent<CharacterHealth>();
                if (_playerHealth != null)
                {
                    // ������������� �� ������� ��������� ��������
                    _playerHealth.OnHealthChange += OnHealthChange;
                    _playerHealth.OnMaxHealthChange += OnMaxHealthChange;

                    // �������������� UI ��� ������
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
            // ������������ �� ������� ��� ����������� �������
            if (_playerHealth != null)
            {
                _playerHealth.OnHealthChange -= OnHealthChange;
                _playerHealth.OnMaxHealthChange -= OnMaxHealthChange;
            }
        }

        private void OnHealthChange(int newValue)
        {
            // ���������� ��� ������������ �������� ������� � ����������� �� �������� ��������
            for (int i = 0; i < _hpIcons.Count; i++)
            {
                _hpIcons[i].SetActive(i < newValue);
            }
        }

        private void OnMaxHealthChange(int newValue)
        {
            // ������� ��� �������� �������
            foreach (GameObject go in _hpIcons)
            {
                Destroy(go);
            }

            // ������� ����� ������ ��������
            for (int i = 0; i < newValue; i++)
            {
                var go = Instantiate(HpPrefab, transform);
                go.transform.localPosition = HpOffset * i;
                _hpIcons.Add(go);
            }

            // ��������� ������� ��������� ��������
            OnHealthChange(_playerHealth.Health);
        }
    }
}