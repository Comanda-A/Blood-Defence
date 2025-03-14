using UnityEngine;
using System.Collections;

namespace WayOfBlood.Character
{
    public class CharacterSound : MonoBehaviour
    {
        [Header("The sound of footsteps")]
        public AudioClip StepAudio1;    // Звук шага
        public AudioClip StepAudio2;    // Звук шага

        [Header("The sound of a katana strike")]
        public AudioClip AttackAudio;   // Звук атаки

        [Header("Sound of gunfire")]
        public AudioClip ShotAudio;     // Звук выстрела

        [Header("The sound of kicking")]
        public AudioClip KickAudio;     // Звук выстрела

        private AudioSource _audioSource;               // AudioSource
        private CharacterMovement _characterMovement;   // Для отслеживания перемещения персонажа
        private bool _isPlayingStepSound = false;       // Флаг для отслеживания воспроизведения звука шагов
        private bool _useStepAudio1 = true;             // Флаг для чередования звуков шагов

        protected virtual void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            // Подписываемся на событие движения
            _characterMovement = GetComponent<CharacterMovement>();

            // Подписываемся на событие перемещения
            if (TryGetComponent<CharacterMovement>(out var componentMovement) 
                && StepAudio1 != null && StepAudio2 != null)
                componentMovement.OnMoving += PlayStepSound;

            // Подписываемся на событие атаки
            if (TryGetComponent<CharacterAttack>(out var componentAttack) && AttackAudio != null)
                componentAttack.OnAttack += PlayAttackSound;

            // Подписываемся на событие выстрела
            if (TryGetComponent<CharacterShot>(out var componentShot) && ShotAudio != null)
                componentShot.OnShot += PlayShotSound;

            // Подписываемся на событие удара ногой
            if (TryGetComponent<СharacterKick>(out var componentKick) && KickAudio != null)
                componentKick.OnKick += PlayKickSound;
        }

        public void PlayOneShot(AudioClip clip) => _audioSource.PlayOneShot(clip);

        // Метод для воспроизведения звука удара ногой
        public void PlayKickSound() => PlayOneShot(KickAudio);

        // Метод для воспроизведения звука атаки
        public void PlayShotSound() => PlayOneShot(ShotAudio);

        // Метод для воспроизведения звука атаки
        public void PlayAttackSound() => PlayOneShot(AttackAudio);

        // Метод для воспроизведения звука шагов
        public void PlayStepSound(Vector2 vector2)
        {
            // Если звук уже воспроизводится, выходим из метода
            if (_isPlayingStepSound)
            {
                return;
            }

            // Выбираем звук для воспроизведения
            AudioClip stepClip;
            if (StepAudio1 != null & StepAudio2 != null)
                stepClip = _useStepAudio1 ? StepAudio1 : StepAudio2;
            else if (StepAudio1 != null)
                stepClip = StepAudio1;
            else if (StepAudio2 != null)
                stepClip = StepAudio2;
            else
                return;

            _useStepAudio1 = !_useStepAudio1; // Меняем флаг для следующего шага

            // Воспроизводим звук
            _audioSource.PlayOneShot(stepClip);

            // Устанавливаем флаг, что звук воспроизводится
            _isPlayingStepSound = true;

            // Запускаем корутину для сброса флага после завершения звука
            StartCoroutine(ResetStepSoundFlag(stepClip.length));
        }

        // Корутина для сброса флага после завершения звука шагов
        private IEnumerator ResetStepSoundFlag(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            _isPlayingStepSound = false;
        }

        private void OnDestroy()
        {
            // Отписываемся при уничтожении компонента

            if (TryGetComponent<CharacterMovement>(out var componentMovement))
                componentMovement.OnMoving -= PlayStepSound;

            if (TryGetComponent<CharacterAttack>(out var componentAttack))
                componentAttack.OnAttack -= PlayAttackSound;

            if (TryGetComponent<CharacterShot>(out var componentShot))
                componentShot.OnShot -= PlayShotSound;

            if (TryGetComponent<СharacterKick>(out var componentKick))
                componentKick.OnKick -= PlayKickSound;
        }
    }
}
