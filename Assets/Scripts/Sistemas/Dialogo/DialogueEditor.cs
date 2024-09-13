using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueManager))]
public class DialogueManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Desenha o inspector padrão
        DrawDefaultInspector();

        // Adiciona um espaço
        GUILayout.Space(10);

        // Desenha uma linha horizontal
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
    }
}
