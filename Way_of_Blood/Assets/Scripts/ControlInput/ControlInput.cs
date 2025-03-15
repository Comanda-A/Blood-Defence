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
        /// Событие, вызываемое при изменении типа управления.
        /// </summary>
        public event UnityAction<InputType> OnInputTypeChanged;

        /// <summary>
        /// Текущий способ управления.
        /// </summary>
        public InputType CurrentInputType { get; private set; } = InputType.Keyboard;

        public TMP_Dropdown InputTypeDropdown;

        private void Start()
        {
            InputTypeDropdown?.onValueChanged.AddListener(SetInputMethodById);
        }

        /// <summary>
        /// Установить способ управления.
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
        /// Способ управления.
        /// </summary>
        public enum InputType
        {
            Keyboard = 0,
            Gamepad = 1,
            Touch = 2
        }
    }


}
