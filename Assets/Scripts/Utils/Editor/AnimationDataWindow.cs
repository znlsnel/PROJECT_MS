using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationDataWindow : EditorWindow
{
    private Vector2 scrollPosition = Vector2.zero;
    private AnimationDataSO animationDataSO;
    private const string EditorPrefsKey = "AnimationDataSO_GUID";
    
    [MenuItem("CustomEditor/Animation Data")]
    public static void ShowWindow()
    {
        GetWindow<AnimationDataWindow>("Animation Data");
    }

    private void OnEnable()
    {
        // 저장된 AnimationDataSO 불러오기
        string guid = EditorPrefs.GetString(EditorPrefsKey, string.Empty);
        if (!string.IsNullOrEmpty(guid))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(assetPath))
            {
                animationDataSO = AssetDatabase.LoadAssetAtPath<AnimationDataSO>(assetPath);
            }
        }
    }

    private void OnDisable()
    {
        // AnimationDataSO GUID 저장
        if (animationDataSO != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(animationDataSO);
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            EditorPrefs.SetString(EditorPrefsKey, guid);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        
        // AnimationDataSO 설정 섹션
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Animation Data SO 설정", EditorStyles.boldLabel);
        
        // ObjectField의 값이 변경되면 자동으로 AnimationDataManager에 설정
        animationDataSO = (AnimationDataSO)EditorGUILayout.ObjectField(
            "Animation Data SO", 
            animationDataSO, 
            typeof(AnimationDataSO), 
            false
        );
        
        EditorGUILayout.HelpBox("AnimationDataSO를 위 필드에 넣으면 자동으로 설정됩니다.", MessageType.Info);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Reset Data"))
        {
            if (EditorUtility.DisplayDialog("Reset Animation Data", 
                "Are you sure you want to reset all animation data?", "Yes", "No"))
            {
                animationDataSO.ResetData();
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
                        animationDataSO.AddAnimationClip(clip);
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

        var animationDataList = animationDataSO.GetAnimationClips();
        
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
