using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WayOfBlood.Character;

namespace WayOfBlood.UI
{
    public class BloodlustUI : MonoBehaviour
    {
        [SerializeField] private Image bloodlustStripe;

        private CharacterBloodlust characterBloodlust;

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && bloodlustStripe != null)
            {
                characterBloodlust = player.GetComponent<CharacterBloodlust>();
                if (characterBloodlust != null)
                {
                    characterBloodlust.OnBloodlustChange += UpdateBloodlustStripe;
                    characterBloodlust.OnMaxBloodlustChange += UpdateBloodlustStripe;
                    UpdateBloodlustStripe(characterBloodlust.Bloodlust);
                }
                else
                {
                    Debug.LogError("CharacterBloodlust component not found on player!");
                }
            }
            else
            {
                Debug.LogError("Player or BloodlustStripe is not assigned!");
            }
        }

        private void UpdateBloodlustStripe(int value)
        {
            bloodlustStripe.fillAmount = (float)characterBloodlust.Bloodlust / characterBloodlust.MaxBloodlust;
        }
    }
}