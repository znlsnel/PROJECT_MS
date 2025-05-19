using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationDataWindow : EditorWindow
{
    private Vector2 scrollPosition = Vector2.zero;
    
    [MenuItem("CustomEditor/Animation Data")]
    public static void ShowWindow()
    {
        GetWindow<AnimationDataWindow>("Animation Data");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Reset Data"))
        {
            if (EditorUtility.DisplayDialog("Reset Animation Data", 
                "Are you sure you want to reset all animation data?", "Yes", "No"))
            {
                AnimationDataManager.ResetData();
            }
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Register All Animation Clips"))
        {
            try
            {
                string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { "Assets" });
                int registeredCount = 0;
                
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                    if (clip != null)
                    {
                        AnimationDataManager.AddAnimationClip(clip);
                        registeredCount++;
                    }
                }
                
                EditorUtility.DisplayDialog("Registration Complete", 
                    $"Successfully registered {registeredCount} animation clips.", "OK");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error registering animation clips: {e.Message}");
                EditorUtility.DisplayDialog("Registration Error", 
                    "An error occurred while registering animation clips. Check console for details.", "OK");
            }
        }

        EditorGUILayout.Space(10);

        var animationDataList = AnimationDataManager.GetAnimationClips();
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        bool showList = EditorGUILayout.Foldout(true, "Animation Clips", EditorStyles.boldLabel);
        
        if (showList)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            if (animationDataList != null && animationDataList.Count > 0)
            {
                for (int i = 0; i < animationDataList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Index {i}", GUILayout.Width(60));
                    EditorGUILayout.ObjectField(animationDataList[i], typeof(AnimationClip), false);
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("No animation clips registered");
            }
            
            EditorGUILayout.EndScrollView();
        }
        
        EditorGUILayout.EndVertical();
    }
}
