using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WayOfBlood.ControlInputSystem
{
    public class ControlInput : MonoBehaviour
    {
        /// <summary>
        /// �������, ���������� ��� ��������� ���� ����������.
        /// </summary>
        public event UnityAction<InputType> OnInputTypeChanged;

        /// <summary>
        /// ������� ������ ����������.
        /// </summary>
        public InputType CurrentInputType { get; private set; } = InputType.Keyboard;

        public TMP_Dropdown InputTypeDropdown;

        private void Start()
        {
            InputTypeDropdown?.onValueChanged.AddListener(SetInputMethodById);
        }

        /// <summary>
        /// ���������� ������ ����������.
        /// </summary>
        public void SetInputMethod(InputType method)
        {
            if (CurrentInputType != method)
            {
                CurrentInputType = method;
                OnInputTypeChanged?.Invoke(CurrentInputType);
                EditorUtility.SetDirty(this);
            }
        }

        public void SetInputMethodById(int id) => SetInputMethod((InputType)id);

        /// <summary>
        /// ������ ����������.
        /// </summary>
        public enum InputType
        {
            Keyboard = 0,
            Gamepad = 1,
            Touch = 2
        }
    }


}
