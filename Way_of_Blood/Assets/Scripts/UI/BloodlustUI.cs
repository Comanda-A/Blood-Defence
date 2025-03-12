using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WayOfBlood.Character;

namespace WayOfBlood.UI
{
    public class BloodlustUI : MonoBehaviour
    {
        [SerializeField] private Image bloodlustStripe;

        private CharacterBlood characterBloodlust;

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && bloodlustStripe != null)
            {
                characterBloodlust = player.GetComponent<CharacterBlood>();
                if (characterBloodlust != null)
                {
                    characterBloodlust.OnBloodChange += UpdateBloodlustStripe;
                    characterBloodlust.OnMaxBloodChange += UpdateBloodlustStripe;
                    UpdateBloodlustStripe(characterBloodlust.Blood);
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
            bloodlustStripe.fillAmount = (float)characterBloodlust.Blood / characterBloodlust.MaxBlood;
        }
    }
}