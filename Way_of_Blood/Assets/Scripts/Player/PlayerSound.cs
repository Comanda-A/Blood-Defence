using UnityEngine;
using System.Collections;

namespace WayOfBlood.Player
{
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField] private AudioClip stepAudio1, stepAudio2; // Два звука шагов
        [SerializeField] private AudioClip attackAudio; // Звук атаки
        [SerializeField] private AudioClip shotAudio; // Звук выстрела

        [SerializeField] private AudioSource audioSource;

        private bool isPlayingStepSound = false; // Флаг для отслеживания воспроизведения звука шагов
        private bool useStepAudio1 = true; // Флаг для чередования звуков шагов

        private void Start()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Подписываемся на событие движения
            GetComponent<PlayerMovement>().OnMoving += PlayStepSound;

            // Подписываемся на событие атаки
            GetComponent<PlayerAttack>().OnAttack += PlayAttackSound;
        }

        // Метод для воспроизведения звука атаки
        public void PlayShotSound()
        {
            audioSource.PlayOneShot(shotAudio);
        }

        // Метод для воспроизведения звука атаки
        public void PlayAttackSound()
        {
            audioSource.PlayOneShot(attackAudio);
        }

        // Метод для воспроизведения звука шагов
        public void PlayStepSound(Vector2 move)
        {
            // Если звук уже воспроизводится, выходим из метода
            if (isPlayingStepSound)
            {
                return;
            }

            // Выбираем звук для воспроизведения
            AudioClip stepClip = useStepAudio1 ? stepAudio1 : stepAudio2;
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