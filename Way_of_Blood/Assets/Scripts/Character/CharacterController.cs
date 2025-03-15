using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character
{
    public class CharacterController : MonoBehaviour
    {
        /// <summary>
        /// �������, ���������� ��� ������ ��������.
        /// </summary>
        public event UnityAction OnDeath;

        /// <summary>
        /// �������� �������.
        /// </summary>
        public bool IsDead { private set; get; }

        protected virtual void Start()
        {
            IsDead = false;
        }

        /// <summary>
        /// ����� ���������.
        /// </summary>
        public virtual void Die()
        {
            if (!IsDead)
            {
                IsDead = true;
                OnDeath?.Invoke(); 
            }        
        }

        protected virtual void OnDestroy()
        {
            OnDeath = null;
        }
    }
}
