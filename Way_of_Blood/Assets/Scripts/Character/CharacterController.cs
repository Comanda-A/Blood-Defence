using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character
{
    public class CharacterController : MonoBehaviour
    {
        /// <summary>
        /// Событие, вызываемое при смерти сущности.
        /// </summary>
        public event UnityAction OnDeath;

        /// <summary>
        /// Сущность погибла.
        /// </summary>
        public bool IsDead { private set; get; }

        protected virtual void Start()
        {
            IsDead = false;
        }

        /// <summary>
        /// Убить сущьность.
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
