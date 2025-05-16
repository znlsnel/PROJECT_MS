using UnityEditor;
using UnityEngine;

public class ItemCreateWindow : EditorWindow
{
    private GameObject draggedPrefab;
    private Vector2 scrollPosition;
    private bool isDragging = false;
    private Rect dropArea;

    public static void ShowWindow()
    {
        GetWindow<ItemCreateWindow>("아이템 만들기");
    }

    void OnGUI()
    {
        DrawDragAndDropArea();
        HandleDragAndDropEvents();
        DrawDraggedPrefabInfo();
    }

    private void DrawDragAndDropArea()
    {
        dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "프리팹을 여기에 드래그하세요", EditorStyles.helpBox);

        if (isDragging)
        {
            EditorGUI.DrawRect(dropArea, new Color(0.5f, 0.5f, 0.5f, 0.2f));
        }
    }

    private void HandleDragAndDropEvents()
    {
        Event currentEvent = Event.current;
        switch (currentEvent.type)
        {
            case EventType.DragUpdated:
                HandleDragUpdated(currentEvent);
                break;

            case EventType.DragPerform:
                HandleDragPerform(currentEvent);
                break;

            case EventType.DragExited:
                isDragging = false;
                break;
        }
    }

    private void HandleDragUpdated(Event currentEvent)
    {
        if (dropArea.Contains(currentEvent.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            isDragging = true;
            currentEvent.Use();
        }
    }

    private void HandleDragPerform(Event currentEvent)
    {
        if (dropArea.Contains(currentEvent.mousePosition))
        {
            DragAndDrop.AcceptDrag();
            isDragging = false;
            ProcessDroppedObjects();
            currentEvent.Use();
        }
    }

    private void ProcessDroppedObjects()
    {
        foreach (Object draggedObject in DragAndDrop.objectReferences)
        {
            if (draggedObject is GameObject)
            {
                draggedPrefab = (GameObject)draggedObject;
                Debug.Log($"드롭된 프리팹: {draggedPrefab.name}");
                // 여기에 프리팹 처리 로직 추가
            }
        }
    }

    private void DrawDraggedPrefabInfo()
    {
        if (draggedPrefab != null)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("드롭된 프리팹:", EditorStyles.boldLabel);
            EditorGUILayout.ObjectField(draggedPrefab, typeof(GameObject), false);
        }
    }
}
