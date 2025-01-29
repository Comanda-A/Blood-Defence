using UnityEngine;
using UnityEngine.Events;


namespace WayOfBlood.Character
{
    public class CharacterBloodlust : MonoBehaviour
    {
        public event UnityAction<int> OnMaxBloodlustChange;    // <int> is new value
        public event UnityAction<int> OnBloodlustChange;       // <int> is new value

        [SerializeField] private int _maxBloodlust;
        public int MaxBloodlust
        {
            private set { _maxBloodlust = value; OnMaxBloodlustChange?.Invoke(_maxBloodlust); }
            get { return _maxBloodlust; }
        }

        [SerializeField] private int _bloodlust;
        public int Bloodlust
        {
            private set { _bloodlust = value; OnBloodlustChange?.Invoke(_bloodlust); }
            get { return _bloodlust; }
        }

        public void AddBloodlust(int value) // добавить жажду крови
        {
            Bloodlust = Bloodlust + value <= MaxBloodlust ? Bloodlust + value : MaxBloodlust;
        }

        public void TakeBloodlust(int value) // взять жажду крови (уменьшить)
        {
            Bloodlust = Bloodlust >= value ? Bloodlust - value : 0;
        }
    }
}
