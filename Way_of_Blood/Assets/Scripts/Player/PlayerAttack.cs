using UnityEngine;
using WayOfBlood.Player.Input;

namespace WayOfBlood.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public event UnityEngine.Events.UnityAction OnAttack;

        public bool Attack { private set; get; }

        private PlayerInputBase playerInputSystem;

        private void Start()
        {
            playerInputSystem = GetComponent<PlayerInputBase>();
            Attack = false;
        }

        private void Update()
        {
            Attack = playerInputSystem.AttackKeyPressed;
            if (Attack) OnAttack?.Invoke();
        }
    }
}
