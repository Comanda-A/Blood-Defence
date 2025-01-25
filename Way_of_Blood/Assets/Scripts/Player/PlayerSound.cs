using UnityEngine;
using System.Collections;

namespace WayOfBlood.Player
{
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField] private AudioClip stepAudio1, stepAudio2; // ��� ����� �����
        [SerializeField] private AudioClip attackAudio; // ���� �����
        [SerializeField] private AudioClip shotAudio; // ���� ��������

        [SerializeField] private AudioSource audioSource;

        private bool isPlayingStepSound = false; // ���� ��� ������������ ��������������� ����� �����
        private bool useStepAudio1 = true; // ���� ��� ����������� ������ �����

        private void Start()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // ������������� �� ������� ��������
            GetComponent<PlayerMovement>().OnMoving += PlayStepSound;

            // ������������� �� ������� �����
            GetComponent<PlayerAttack>().OnAttack += PlayAttackSound;
        }

        // ����� ��� ��������������� ����� �����
        public void PlayShotSound()
        {
            audioSource.PlayOneShot(shotAudio);
        }

        // ����� ��� ��������������� ����� �����
        public void PlayAttackSound()
        {
            audioSource.PlayOneShot(attackAudio);
        }

        // ����� ��� ��������������� ����� �����
        public void PlayStepSound(Vector2 move)
        {
            // ���� ���� ��� ���������������, ������� �� ������
            if (isPlayingStepSound)
            {
                return;
            }

            // �������� ���� ��� ���������������
            AudioClip stepClip = useStepAudio1 ? stepAudio1 : stepAudio2;
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