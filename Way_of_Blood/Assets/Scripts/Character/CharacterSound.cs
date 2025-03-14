using UnityEngine;
using System.Collections;

namespace WayOfBlood.Character
{
    public class CharacterSound : MonoBehaviour
    {
        [Header("The sound of footsteps")]
        public AudioClip StepAudio1;    // ���� ����
        public AudioClip StepAudio2;    // ���� ����

        [Header("The sound of a katana strike")]
        public AudioClip AttackAudio;   // ���� �����

        [Header("Sound of gunfire")]
        public AudioClip ShotAudio;     // ���� ��������

        [Header("The sound of kicking")]
        public AudioClip KickAudio;     // ���� ��������

        private AudioSource _audioSource;               // AudioSource
        private CharacterMovement _characterMovement;   // ��� ������������ ����������� ���������
        private bool _isPlayingStepSound = false;       // ���� ��� ������������ ��������������� ����� �����
        private bool _useStepAudio1 = true;             // ���� ��� ����������� ������ �����

        protected virtual void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            // ������������� �� ������� ��������
            _characterMovement = GetComponent<CharacterMovement>();

            // ������������� �� ������� �����������
            if (TryGetComponent<CharacterMovement>(out var componentMovement) 
                && StepAudio1 != null && StepAudio2 != null)
                componentMovement.OnMoving += PlayStepSound;

            // ������������� �� ������� �����
            if (TryGetComponent<CharacterAttack>(out var componentAttack) && AttackAudio != null)
                componentAttack.OnAttack += PlayAttackSound;

            // ������������� �� ������� ��������
            if (TryGetComponent<CharacterShot>(out var componentShot) && ShotAudio != null)
                componentShot.OnShot += PlayShotSound;

            // ������������� �� ������� ����� �����
            if (TryGetComponent<�haracterKick>(out var componentKick) && KickAudio != null)
                componentKick.OnKick += PlayKickSound;
        }

        public void PlayOneShot(AudioClip clip) => _audioSource.PlayOneShot(clip);

        // ����� ��� ��������������� ����� ����� �����
        public void PlayKickSound() => PlayOneShot(KickAudio);

        // ����� ��� ��������������� ����� �����
        public void PlayShotSound() => PlayOneShot(ShotAudio);

        // ����� ��� ��������������� ����� �����
        public void PlayAttackSound() => PlayOneShot(AttackAudio);

        // ����� ��� ��������������� ����� �����
        public void PlayStepSound(Vector2 vector2)
        {
            // ���� ���� ��� ���������������, ������� �� ������
            if (_isPlayingStepSound)
            {
                return;
            }

            // �������� ���� ��� ���������������
            AudioClip stepClip;
            if (StepAudio1 != null & StepAudio2 != null)
                stepClip = _useStepAudio1 ? StepAudio1 : StepAudio2;
            else if (StepAudio1 != null)
                stepClip = StepAudio1;
            else if (StepAudio2 != null)
                stepClip = StepAudio2;
            else
                return;

            _useStepAudio1 = !_useStepAudio1; // ������ ���� ��� ���������� ����

            // ������������� ����
            _audioSource.PlayOneShot(stepClip);

            // ������������� ����, ��� ���� ���������������
            _isPlayingStepSound = true;

            // ��������� �������� ��� ������ ����� ����� ���������� �����
            StartCoroutine(ResetStepSoundFlag(stepClip.length));
        }

        // �������� ��� ������ ����� ����� ���������� ����� �����
        private IEnumerator ResetStepSoundFlag(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            _isPlayingStepSound = false;
        }

        private void OnDestroy()
        {
            // ������������ ��� ����������� ����������

            if (TryGetComponent<CharacterMovement>(out var componentMovement))
                componentMovement.OnMoving -= PlayStepSound;

            if (TryGetComponent<CharacterAttack>(out var componentAttack))
                componentAttack.OnAttack -= PlayAttackSound;

            if (TryGetComponent<CharacterShot>(out var componentShot))
                componentShot.OnShot -= PlayShotSound;

            if (TryGetComponent<�haracterKick>(out var componentKick))
                componentKick.OnKick -= PlayKickSound;
        }
    }
}
