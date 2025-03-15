using UnityEditor;

namespace WayOfBlood.ControlInputSystem
{
    [CustomEditor(typeof(ControlInput))]
    public class ControlInputEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // ������ ����������� ��������� ��� ����������
            base.OnInspectorGUI();

            ControlInput controlInput = (ControlInput)target;

            // ��������� ���������
            EditorGUILayout.LabelField("Input type", EditorStyles.boldLabel);

            // ������� ���������� ������ ��� ��������� ���� ����������
            ControlInput.InputType newInputType = (ControlInput.InputType)EditorGUILayout.EnumPopup("Input Method", controlInput.CurrentInputType);

            // ���� ������ ����� ��� ����������, ��������� ���
            if (newInputType != controlInput.CurrentInputType)
            {
                // ������� ����� ��� ��������� ���� ����������
                controlInput.SetInputMethod(newInputType);
            }
        }
    }
}
