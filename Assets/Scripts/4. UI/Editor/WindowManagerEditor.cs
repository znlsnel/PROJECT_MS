using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WindowManager))]
public class WindowManagerEditor : Editor
{
    private int currentTab;
    private SerializedProperty managementType;
    private SerializedProperty groupName;
    private SerializedProperty onChangeWindow;
    private SerializedProperty onOpenWindow;
    private SerializedProperty onCloseWindow;
    private SerializedProperty windowItems;

    public void OnEnable()
    {
        managementType = serializedObject.FindProperty("managementType");
        groupName = serializedObject.FindProperty("groupName");
        onChangeWindow = serializedObject.FindProperty("onChangeWindow");
        onOpenWindow = serializedObject.FindProperty("onOpenWindow");
        onCloseWindow = serializedObject.FindProperty("onCloseWindow");
        windowItems = serializedObject.FindProperty("windowItems");
    }

    public override void OnInspectorGUI()
    {
        var boldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        boldLabelStyle.fontSize = 36;
        boldLabelStyle.fixedHeight = 36;

        serializedObject.Update();

        GUILayout.Space(25);
        EditorGUILayout.LabelField("Window Manager", boldLabelStyle, GUILayout.Height(36));
        GUILayout.Space(25);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("내용", "창 내용 관리")))
            currentTab = 0;
        if (GUILayout.Button(new GUIContent("설정", "동작 설정")))
            currentTab = 1;
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        switch(currentTab)
        {
            case 0:
                ContentTab();
                break;
            case 1:
                SettingTab();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ContentTab()
    {
        EditorGUILayout.PropertyField(managementType, new GUIContent("관리 타입"));

        EditorGUILayout.PropertyField(windowItems, new GUIContent("Window Items"));


        EditorGUILayout.LabelField("Event", EditorStyles.boldLabel);

        switch(managementType.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(onOpenWindow, new GUIContent("열기 이벤트"));
                EditorGUILayout.PropertyField(onCloseWindow, new GUIContent("닫기 이벤트"));
                break;
            case 1:
                EditorGUILayout.PropertyField(onChangeWindow, new GUIContent("변경 이벤트"));
                break;
        }
    }

    public void SettingTab()
    {
        
    }
}
