using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationDataWindow : EditorWindow
{
    [MenuItem("CustomEditor/Animation Data")]
    public static void ShowWindow()
    {
        GetWindow<AnimationDataWindow>("Animation Data");
    }

    private void OnGUI()
    {
        var animationDataList = AnimationDataManager.GetAnimationClips();
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Animation Clips", EditorStyles.boldLabel);
        
        for (int i = 0; i < animationDataList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Index {i}", GUILayout.Width(60));
            EditorGUILayout.ObjectField(animationDataList[i], typeof(AnimationClip), false);
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Reset Data"))
        {
            AnimationDataManager.ResetData();
        }
    }
}
