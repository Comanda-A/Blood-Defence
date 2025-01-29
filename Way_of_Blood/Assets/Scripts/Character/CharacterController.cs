using UnityEngine;
using UnityEngine.Events;

namespace WayOfBlood.Character
{
    public class CharacterController : MonoBehaviour
    {
        public event UnityAction OnDeath;

        public bool isDead { private set; get; }

        protected virtual void Start()
        {
            isDead = false;
        }

        public virtual void Die()
        {
            if (!isDead)
            {
                isDead = true;
                OnDeath?.Invoke(); 
            }        
        }

        protected virtual void OnDestroy()
        {
            OnDeath = null;
        }
    }
}
