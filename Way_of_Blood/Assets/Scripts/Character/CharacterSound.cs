using UnityEngine;
using System.Collections;

namespace WayOfBlood.Character
{
    public class CharacterSound : MonoBehaviour
    {
        public AudioClip StepAudio1;    // Звук шага
        public AudioClip StepAudio2;    // Звук шага
        public AudioClip AttackAudio;   // Звук атаки
        public AudioClip ShotAudio;     // Звук выстрела
        public AudioClip KickAudio;     // Звук выстрела

        [SerializeField] private AudioSource audioSource;

        private bool isPlayingStepSound = false;    // Флаг для отслеживания воспроизведения звука шагов
        private bool useStepAudio1 = true;          // Флаг для чередования звуков шагов
        private CharacterMovement characterMovement;

        protected virtual void Start()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Подписываемся на событие движения
            characterMovement = GetComponent<CharacterMovement>();

            // Подписываемся на событие атаки
            GetComponent<CharacterAttack>().OnAttack += PlayAttackSound;

            // Подписываемся на событие выстрела
            GetComponent<CharacterShot>().OnShot += PlayShotSound;

            if (TryGetComponent<СharacterKick>(out var сharacterKick))
            {
                сharacterKick.OnKick += PlayKickSound;
            }
        }

        private void Update()
        {
            if (characterMovement.MoveDirection != null)
                PlayStepSound();
        }

        public void PlaySound(AudioClip audio)
        {
            audioSource.PlayOneShot(audio);
        }    

        // Метод для воспроизведения звука удара ногой
        public void PlayKickSound()
        {
            if (KickAudio != null)
            {
                audioSource.PlayOneShot(KickAudio);
            }
        }

        // Метод для воспроизведения звука атаки
        public void PlayShotSound()
        {
            if (ShotAudio != null)
            {
                audioSource.PlayOneShot(ShotAudio);
            } 
        }

        // Метод для воспроизведения звука атаки
        public void PlayAttackSound()
        {
            if (AttackAudio != null)
            {
                audioSource.PlayOneShot(AttackAudio);
            }  
        }

        // Метод для воспроизведения звука шагов
        public void PlayStepSound()
        {
            // Если звук уже воспроизводится, выходим из метода
            if (isPlayingStepSound)
            {
                return;
            }

            // Выбираем звук для воспроизведения
            AudioClip stepClip;
            if (StepAudio1 != null & StepAudio2 != null)
                stepClip = useStepAudio1 ? StepAudio1 : StepAudio2;
            else if (StepAudio1 != null)
                stepClip = StepAudio1;
            else if (StepAudio2 != null)
                stepClip = StepAudio2;
            else
                return;

            useStepAudio1 = !useStepAudio1; // Меняем флаг для следующего шага

            // Воспроизводим звук
            audioSource.PlayOneShot(stepClip);

            // Устанавливаем флаг, что звук воспроизводится
            isPlayingStepSound = true;

            // Запускаем корутину для сброса флага после завершения звука
            StartCoroutine(ResetStepSoundFlag(stepClip.length));
        }

        // Корутина для сброса флага после завершения звука шагов
        private IEnumerator ResetStepSoundFlag(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            isPlayingStepSound = false;
        }
    }
}
