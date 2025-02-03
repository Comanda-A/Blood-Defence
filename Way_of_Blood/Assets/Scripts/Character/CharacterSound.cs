using UnityEngine;
using System.Collections;

namespace WayOfBlood.Character
{
    public class CharacterSound : MonoBehaviour
    {
        public AudioClip StepAudio1;    // ���� ����
        public AudioClip StepAudio2;    // ���� ����
        public AudioClip AttackAudio;   // ���� �����
        public AudioClip ShotAudio;     // ���� ��������
        public AudioClip KickAudio;     // ���� ��������

        [SerializeField] private AudioSource audioSource;

        private bool isPlayingStepSound = false;    // ���� ��� ������������ ��������������� ����� �����
        private bool useStepAudio1 = true;          // ���� ��� ����������� ������ �����
        private CharacterMovement characterMovement;

        protected virtual void Start()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // ������������� �� ������� ��������
            characterMovement = GetComponent<CharacterMovement>();

            // ������������� �� ������� �����
            GetComponent<CharacterAttack>().OnAttack += PlayAttackSound;

            // ������������� �� ������� ��������
            GetComponent<CharacterShot>().OnShot += PlayShotSound;

            if (TryGetComponent<�haracterKick>(out var �haracterKick))
            {
                �haracterKick.OnKick += PlayKickSound;
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

        // ����� ��� ��������������� ����� ����� �����
        public void PlayKickSound()
        {
            if (KickAudio != null)
            {
                audioSource.PlayOneShot(KickAudio);
            }
        }

        // ����� ��� ��������������� ����� �����
        public void PlayShotSound()
        {
            if (ShotAudio != null)
            {
                audioSource.PlayOneShot(ShotAudio);
            } 
        }

        // ����� ��� ��������������� ����� �����
        public void PlayAttackSound()
        {
            if (AttackAudio != null)
            {
                audioSource.PlayOneShot(AttackAudio);
            }  
        }

        // ����� ��� ��������������� ����� �����
        public void PlayStepSound()
        {
            // ���� ���� ��� ���������������, ������� �� ������
            if (isPlayingStepSound)
            {
                return;
            }

            // �������� ���� ��� ���������������
            AudioClip stepClip;
            if (StepAudio1 != null & StepAudio2 != null)
                stepClip = useStepAudio1 ? StepAudio1 : StepAudio2;
            else if (StepAudio1 != null)
                stepClip = StepAudio1;
            else if (StepAudio2 != null)
                stepClip = StepAudio2;
            else
                return;

            useStepAudio1 = !useStepAudio1; // ������ ���� ��� ���������� ����

            // ������������� ����
            audioSource.PlayOneShot(stepClip);

            // ������������� ����, ��� ���� ���������������
            isPlayingStepSound = true;

            // ��������� �������� ��� ������ ����� ����� ���������� �����
            StartCoroutine(ResetStepSoundFlag(stepClip.length));
        }

        // �������� ��� ������ ����� ����� ���������� ����� �����
        private IEnumerator ResetStepSoundFlag(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            isPlayingStepSound = false;
        }
    }
}
