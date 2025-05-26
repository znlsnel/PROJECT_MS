#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorSO))]
public class ColorManagerEditor : Editor
{
    private ColorSO colorManager;
    private bool showHexCodes = false;
    private bool foldoutColors = true;

    private void OnEnable()
    {
        colorManager = target as ColorSO;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        // 헤더
        EditorGUILayout.Space(10);
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter
        };
        EditorGUILayout.LabelField("🎨 Color Manager", headerStyle);
        EditorGUILayout.Space(10);

        // 설정 버튼들
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("기본 색상으로 초기화", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("색상 초기화", "모든 색상을 기본값으로 초기화하시겠습니까?", "예", "아니요"))
            {
                colorManager.ResetToDefaultColors();
                EditorUtility.SetDirty(colorManager);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        // 색상 설정 영역
        foldoutColors = EditorGUILayout.Foldout(foldoutColors, "색상 설정", true);
        if (foldoutColors)
        {
            EditorGUILayout.BeginVertical("box");
            DrawColorSettings();
            EditorGUILayout.EndVertical();
        }

        // 색상 미리보기
        EditorGUILayout.Space(10);

        // 변경사항 적용
        if (EditorGUI.EndChangeCheck())
        {
            colorManager.OnColorsChanged();
            EditorUtility.SetDirty(colorManager);
        }
    }

    private void DrawColorSettings()
    {
        ColorType[] colorTypes = (ColorType[])Enum.GetValues(typeof(ColorType));
        Color[] colors = colorManager.Colors;

        // 배열 크기 확인 및 조정
        if (colors.Length != colorTypes.Length)
        {
            Array.Resize(ref colors, colorTypes.Length);
            colorManager.Colors = colors;
        }

        for (int i = 0; i < colorTypes.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // 색상 이름 라벨
            string labelText = GetKoreanColorName(colorTypes[i]);
            EditorGUILayout.LabelField($"{colorTypes[i]} ({labelText})", GUILayout.Width(150));

            // 색상 필드
            Color newColor = EditorGUILayout.ColorField(colors[i]);
            if (newColor != colors[i])
            {
                colors[i] = newColor;
                colorManager.Colors = colors;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
        }
    }

    private string GetKoreanColorName(ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Black => "검정",
            ColorType.Gray => "회색",
            ColorType.White => "흰색",
            ColorType.Red => "빨강",
            ColorType.Orange => "주황",
            ColorType.Yellow => "노랑",
            ColorType.LightGreen => "연두",
            ColorType.Green => "초록",
            ColorType.Turquoise => "청록",
            ColorType.Blue => "파랑",
            ColorType.Indigo => "남색",
            ColorType.Purple => "보라",
            ColorType.Pink => "분홍",
            ColorType.Brown => "갈색",
            ColorType.Gold => "금색",
            ColorType.Mint => "민트",
            _ => "알 수 없음"
        };
    }

    [MenuItem("Assets/Create/Managers/ColorManager Asset")]
    public static void CreateColorManagerAsset()
    {
        ColorSO asset = ScriptableObject.CreateInstance<ColorSO>();
        asset.ResetToDefaultColors();
        
        string path = EditorUtility.SaveFilePanelInProject(
            "Create ColorManager Asset",
            "ColorManager",
            "asset",
            "색상 관리자 에셋을 저장할 위치를 선택하세요.");
        
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
#endif 