using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 표시
        DrawDefaultInspector();

        Test test = (Test)target;

        GUILayout.Space(10);

        if (GUILayout.Button("카드생성"))
        {
            test.ButtonAction1();
        }

        if (GUILayout.Button("배치"))
        {
            test.ButtonAction2();
        }
    }
}