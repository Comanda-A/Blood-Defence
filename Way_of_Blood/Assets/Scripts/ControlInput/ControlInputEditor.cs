using UnityEditor;

namespace WayOfBlood.ControlInputSystem
{
    [CustomEditor(typeof(ControlInput))]
    public class ControlInputEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Рисуем стандартный инспектор для компонента
            base.OnInspectorGUI();

            ControlInput controlInput = (ControlInput)target;

            // Добавляем заголовок
            EditorGUILayout.LabelField("Input type", EditorStyles.boldLabel);

            // Выводим выпадающий список для изменения типа управления
            ControlInput.InputType newInputType = (ControlInput.InputType)EditorGUILayout.EnumPopup("Input Method", controlInput.CurrentInputType);

            // Если выбран новый тип управления, обновляем его
            if (newInputType != controlInput.CurrentInputType)
            {
                // Вызовем метод для изменения типа управления
                controlInput.SetInputMethod(newInputType);
            }
        }
    }
}
