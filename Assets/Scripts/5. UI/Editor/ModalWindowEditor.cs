#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using System.Drawing;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

[CustomEditor(typeof(ModalWindow))]
public class ModalWindowEditor : Editor
{
    private GUISkin customSkin;
    private ModalWindow mwTarget;
    private int currentTab;
    private SerializedProperty windowIconImage;
    private SerializedProperty windowIcon;
    private SerializedProperty windowTitleText;
    private SerializedProperty windowTitle;
    private SerializedProperty windowDescriptionText;
    private SerializedProperty windowDescription;
    private SerializedProperty onConfirm;
    private SerializedProperty onCancel;
    private SerializedProperty onOpen;
    private SerializedProperty confirmButton;
    private SerializedProperty cancelButton;
    private SerializedProperty showConfirmButton;
    private SerializedProperty showCancelButton;
    private SerializedProperty closeBehaviour;
    private SerializedProperty closeOnCancel;
    private SerializedProperty closeOnConfirm;

    private void OnEnable()
    {
        mwTarget = (ModalWindow)target;

        // 프로퍼티 찾기
        windowIconImage = serializedObject.FindProperty("windowIconImage");
        windowIcon = serializedObject.FindProperty("windowIcon");
        windowTitleText = serializedObject.FindProperty("windowTitleText");
        windowTitle = serializedObject.FindProperty("windowTitle");
        windowDescriptionText = serializedObject.FindProperty("windowDescriptionText");
        windowDescription = serializedObject.FindProperty("windowDescription");
        onConfirm = serializedObject.FindProperty("onConfirm");
        onCancel = serializedObject.FindProperty("onCancel");
        onOpen = serializedObject.FindProperty("onOpen");
        confirmButton = serializedObject.FindProperty("confirmButton");
        cancelButton = serializedObject.FindProperty("cancelButton");
        showConfirmButton = serializedObject.FindProperty("showConfirmButton");
        showCancelButton = serializedObject.FindProperty("showCancelButton");
        closeBehaviour = serializedObject.FindProperty("closeBehaviour");
        closeOnCancel = serializedObject.FindProperty("closeOnCancel");
        closeOnConfirm = serializedObject.FindProperty("closeOnConfirm");

        if (EditorGUIUtility.isProSkin == true) 
        { 
            customSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene); 
        }
        else 
        { 
            customSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector); 
        }
    }

    public override void OnInspectorGUI()
    {
        var boldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        boldLabelStyle.fontSize = 36;
        boldLabelStyle.fixedHeight = 36;

        serializedObject.Update();

        GUILayout.Space(25);
        EditorGUILayout.LabelField("Modal Window", boldLabelStyle, GUILayout.Height(36));
        GUILayout.Space(25);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("내용", "창 내용 관리"), GetTabButtonStyle("내용")))
            currentTab = 0;
        if (GUILayout.Button(new GUIContent("리소스", "연결된 오브젝트"), GetTabButtonStyle("리소스")))
            currentTab = 1;
        if (GUILayout.Button(new GUIContent("설정", "동작 설정"), GetTabButtonStyle("설정")))
            currentTab = 2;
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        switch (currentTab)
        {
            case 0:
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Icon", GUILayout.Width(120));
                EditorGUILayout.PropertyField(windowIcon, GUIContent.none);
                GUILayout.EndHorizontal();

                if(EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    mwTarget.UpdateContent();
                }
                
                Image iconImageComp = (Image)windowIconImage.objectReferenceValue;
                if(iconImageComp == null)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("'아이콘 이미지 컴포넌트'가 할당되지 않았습니다. '리소스' 탭에서 올바른 변수를 할당하세요.", MessageType.Error);
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.LabelField("Title", GUILayout.Width(120));
                EditorGUILayout.PropertyField(windowTitle, GUIContent.none);
                GUILayout.EndHorizontal();
                
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    mwTarget.UpdateContent();
                }
                
                TextMeshProUGUI titleTextComp = (TextMeshProUGUI)windowTitleText.objectReferenceValue;
                if (titleTextComp == null) 
                { 
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("'제목 텍스트 컴포넌트'가 할당되지 않았습니다. '리소스' 탭에서 올바른 변수를 할당하세요.", MessageType.Error);
                    GUILayout.EndHorizontal();
                }

                EditorGUI.BeginChangeCheck();
                GUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(new GUIContent("Description"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(windowDescription, GUIContent.none, GUILayout.Height(60));
                GUILayout.EndVertical();
                
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    mwTarget.UpdateContent();
                }
                
                TextMeshProUGUI descTextComp = (TextMeshProUGUI)windowDescriptionText.objectReferenceValue;
                if (descTextComp == null) 
                { 
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("'설명 텍스트 컴포넌트'가 할당되지 않았습니다. '리소스' 탭에서 올바른 변수를 할당하세요.", MessageType.Error);
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(5);
                
                if (mwTarget.GetComponent<CanvasGroup>().alpha == 0)
                {
                    if (GUILayout.Button("표시", GUILayout.Height(25)))
                    {
                        mwTarget.Show();
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    }
                }
                else
                {
                    if (GUILayout.Button("숨기기", GUILayout.Height(25)))
                    {
                        mwTarget.Hide();
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    }
                }

                DrawHeader("이벤트", 10);
                EditorGUILayout.PropertyField(onOpen, new GUIContent("열기 이벤트"), true);
                EditorGUILayout.PropertyField(onConfirm, new GUIContent("확인 이벤트"), true);
                EditorGUILayout.PropertyField(onCancel, new GUIContent("취소 이벤트"), true);
                break;

            case 1:
                DrawProperty(windowIconImage, "아이콘 이미지");
                DrawProperty(windowTitleText, "제목 텍스트 컴포넌트");
                DrawProperty(windowDescriptionText, "설명 텍스트 컴포넌트");
                DrawProperty(confirmButton, "확인 버튼");
                DrawProperty(cancelButton, "취소 버튼");
                break;

            case 2:
                EditorGUILayout.PropertyField(closeBehaviour, new GUIContent("닫기 동작"));
                
                EditorGUI.BeginChangeCheck();
                showConfirmButton.boolValue = DrawToggle(showConfirmButton.boolValue, "확인 버튼 표시");
                showCancelButton.boolValue = DrawToggle(showCancelButton.boolValue, "취소 버튼 표시");
                closeOnConfirm.boolValue = DrawToggle(closeOnConfirm.boolValue, "확인 시 창 닫기");
                closeOnCancel.boolValue = DrawToggle(closeOnCancel.boolValue, "취소 시 창 닫기");
                
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    mwTarget.InitButtons();
                }
                break;
        }

        if (Application.isPlaying == false) { this.Repaint(); }
        serializedObject.ApplyModifiedProperties();
    }

    // 헤더 그리기
    private void DrawHeader(string title, float space = 0)
    {
        GUILayout.Space(space);
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        GUILayout.Space(2);
    }

    // 프로퍼티 그리기
    private void DrawProperty(SerializedProperty property, string label, GUIStyle style = null)
    {
        GUILayout.BeginHorizontal();
        
        if (style != null)
            EditorGUILayout.LabelField(new GUIContent(label), style);
        else
            EditorGUILayout.LabelField(new GUIContent(label));
            
        EditorGUILayout.PropertyField(property, GUIContent.none, true);
        
        GUILayout.EndHorizontal();
    }

    // 토글 그리기
    private bool DrawToggle(bool value, string label)
    {
        GUILayout.BeginHorizontal();
        value = EditorGUILayout.Toggle(label, value);
        GUILayout.EndHorizontal();
        return value;
    }

    // 탭 버튼 스타일 가져오기
    private GUIStyle GetTabButtonStyle(string tabName)
    {
        if (currentTab == 0 && tabName == "내용" || 
            currentTab == 1 && tabName == "리소스" || 
            currentTab == 2 && tabName == "설정")
            return EditorStyles.toolbarButton;
        else
            return EditorStyles.miniButton;
    }
}
#endif