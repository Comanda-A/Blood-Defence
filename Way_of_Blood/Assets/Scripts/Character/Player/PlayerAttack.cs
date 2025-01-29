using UnityEngine;
using UnityEngine.XR;
using WayOfBlood.Input;

namespace WayOfBlood.Character.Player
{
    public class PlayerAttack : CharacterAttack
    {
        [Header("Bloodlust bonus on the kill")]
        public int BloodlustBonusOnKill = 1;

        private PlayerBloodlust playerBloodlust;
        private InputBase playerInputSystem;

        protected override void Start()
        {
            base.Start();
            playerBloodlust = GetComponent<PlayerBloodlust>();
            playerInputSystem = GetComponent<InputBase>();
            OnDamage += DamageHandler;
        }

        private void Update()
        {
            if (playerInputSystem.AttackKeyPressed)
                Attack();
        }

        private void DamageHandler(CharacterHealth characterHealth)
        {
            playerBloodlust.AddBloodlust(BloodlustBonusOnKill);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnDamage -= DamageHandler;
        }
    }
}