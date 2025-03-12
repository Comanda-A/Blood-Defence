using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character
{
    public class CharacterHealth : MonoBehaviour
    {
        public event UnityAction<int> OnMaxHealthChange;    // <int> is new value
        public event UnityAction<int> OnHealthChange;       // <int> is new value

        [SerializeField] private int _maxHealth;
        public int MaxHealth
        {
            private set { _maxHealth = value; OnMaxHealthChange?.Invoke(_maxHealth); }
            get { return _maxHealth; }
        }

        [SerializeField] private int _health;
        public int Health
        {
            private set { _health = value; OnHealthChange?.Invoke(_health); }
            get { return _health;  }
        }

        protected CharacterController _characterController;

        protected virtual void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public virtual void AddHealth(int value)
        {
            if (!_characterController.isDead)
                Health = Health + value <= MaxHealth ? Health + value : MaxHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            if (!_characterController.isDead)
                Health = Health >= damage ? Health - damage : 0;
        }

        protected virtual void OnDestroy()
        {
            OnMaxHealthChange = null;
            OnHealthChange = null;
        }
    }
}
