using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class MeshSerttingsBatchProcessor : EditorWindow
{
    private List<GameObject> targetObjects = new List<GameObject>();
    private Vector2 scrollPosition;
    
    // 설정값들
    private ModelImporterNormals normalsMode = ModelImporterNormals.Calculate;
    private ModelImporterNormalCalculationMode normalCalculationMode = ModelImporterNormalCalculationMode.Unweighted_Legacy;
    private float smoothingAngle = 180f;
    private ModelImporterTangents tangentsMode = ModelImporterTangents.CalculateLegacyWithSplitTangents;
    
    [MenuItem("Tools/Mesh Settings Batch Processor")]
    public static void ShowWindow()
    {
        MeshSerttingsBatchProcessor window = GetWindow<MeshSerttingsBatchProcessor>("Mesh Settings Processor");
        window.minSize = new Vector2(400, 500);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Label("Mesh Settings Batch Processor", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // 설정값 UI
        DrawSettingsUI();
        
        GUILayout.Space(10);
        
        // 드래그 앤 드롭 영역
        DrawDragDropArea();
        
        GUILayout.Space(10);
        
        // 오브젝트 리스트
        DrawObjectList();
        
        GUILayout.Space(10);
        
        // 버튼들 
        DrawButtons();
    }
    
    private void DrawSettingsUI()
    {
        GUILayout.Label("Mesh Import Settings", EditorStyles.boldLabel);
        
        // Normals
        normalsMode = (ModelImporterNormals)EditorGUILayout.EnumPopup("Normals", normalsMode);
        
        // Normal Calculation Mode
        normalCalculationMode = (ModelImporterNormalCalculationMode)EditorGUILayout.EnumPopup("Normal Calculation Mode", normalCalculationMode);
        
        // Smoothing Angle
        smoothingAngle = EditorGUILayout.FloatField("Smoothing Angle", smoothingAngle);
        
        // Tangents
        tangentsMode = (ModelImporterTangents)EditorGUILayout.EnumPopup("Tangents", tangentsMode);
    }
    
    private void DrawDragDropArea()
    {
        GUILayout.Label("Drag & Drop Objects Here", EditorStyles.boldLabel);
        
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag GameObjects or Prefabs here");
        
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;
                    
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                
                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    
                    foreach (UnityEngine.Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is GameObject gameObject)
                        {
                            if (!targetObjects.Contains(gameObject))
                            {
                                targetObjects.Add(gameObject);
                            }
                        }
                    }
                }
                break;
        }
    }
    
    private void DrawObjectList()
    {
        GUILayout.Label($"Target Objects ({targetObjects.Count})", EditorStyles.boldLabel);
        
        if (targetObjects.Count > 0)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(150));
            
            for (int i = targetObjects.Count - 1; i >= 0; i--)
            {
                EditorGUILayout.BeginHorizontal();
                
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(targetObjects[i], typeof(GameObject), false);
                EditorGUI.EndDisabledGroup();
                
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    targetObjects.RemoveAt(i);
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.HelpBox("No objects added. Drag and drop GameObjects or Prefabs above.", MessageType.Info);
        }
    }
    
    private void DrawButtons()
    {
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Clear All"))
        {
            targetObjects.Clear();
        }
        
        EditorGUI.BeginDisabledGroup(targetObjects.Count == 0);
        if (GUILayout.Button("Apply Settings"))
        {
            ApplySettingsToObjects();
        }
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void ApplySettingsToObjects()
    {
        int processedCount = 0;
        int totalCount = 0;
        
        try
        {
            EditorUtility.DisplayProgressBar("Processing Meshes", "Extracting meshes from objects...", 0f);
            
            // 모든 오브젝트에서 메시 추출
            List<string> meshPaths = new List<string>();
            
            foreach (GameObject obj in targetObjects)
            {
                ExtractMeshPaths(obj, meshPaths);
            }
            
            totalCount = meshPaths.Count;
            
            if (totalCount == 0)
            {
                EditorUtility.DisplayDialog("No Meshes Found", "No meshes were found in the selected objects.", "OK");
                return;
            }
            
            // 각 메시에 설정 적용
            foreach (string meshPath in meshPaths)
            {
                EditorUtility.DisplayProgressBar("Processing Meshes", 
                    $"Processing: {System.IO.Path.GetFileName(meshPath)}", 
                    (float)processedCount / totalCount);
                
                if (ApplySettingsToMesh(meshPath))
                {
                    processedCount++;
                }
            }
            
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("Process Complete", 
                $"Successfully processed {processedCount} out of {totalCount} meshes.", "OK");
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
    
    private void ExtractMeshPaths(GameObject obj, List<string> meshPaths)
    {
        // 오브젝트의 Asset 경로 가져오기
        string assetPath = AssetDatabase.GetAssetPath(obj);
        
        if (!string.IsNullOrEmpty(assetPath))
        {
            // 프리팹이나 모델 파일인 경우
            if (assetPath.EndsWith(".fbx") || assetPath.EndsWith(".obj") || 
                assetPath.EndsWith(".dae") || assetPath.EndsWith(".3ds") ||
                assetPath.EndsWith(".blend") || assetPath.EndsWith(".ma") ||
                assetPath.EndsWith(".mb") || assetPath.EndsWith(".max"))
            {
                if (!meshPaths.Contains(assetPath))
                {
                    meshPaths.Add(assetPath);
                }
            }
            else
            {
                // 프리팹인 경우, 내부의 메시 컴포넌트들 확인
                MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
                SkinnedMeshRenderer[] skinnedMeshes = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                
                // MeshFilter에서 메시 추출
                foreach (MeshFilter meshFilter in meshFilters)
                {
                    if (meshFilter.sharedMesh != null)
                    {
                        string meshAssetPath = AssetDatabase.GetAssetPath(meshFilter.sharedMesh);
                        if (!string.IsNullOrEmpty(meshAssetPath) && !meshPaths.Contains(meshAssetPath))
                        {
                            meshPaths.Add(meshAssetPath);
                        }
                    }
                }
                
                // SkinnedMeshRenderer에서 메시 추출
                foreach (SkinnedMeshRenderer skinnedMesh in skinnedMeshes)
                {
                    if (skinnedMesh.sharedMesh != null)
                    {
                        string meshAssetPath = AssetDatabase.GetAssetPath(skinnedMesh.sharedMesh);
                        if (!string.IsNullOrEmpty(meshAssetPath) && !meshPaths.Contains(meshAssetPath))
                        {
                            meshPaths.Add(meshAssetPath);
                        }
                    }
                }
            }
        }
    }
    
    private bool ApplySettingsToMesh(string assetPath)
    {
        try
        {
            ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            
            if (importer == null)
            {
                Debug.LogWarning($"Could not get ModelImporter for: {assetPath}");
                return false;
            }
            
            // 설정값 적용
            importer.importNormals = normalsMode;
            importer.normalCalculationMode = normalCalculationMode;
            importer.normalSmoothingAngle = smoothingAngle;
            importer.importTangents = tangentsMode;
            
            // 변경사항 저장 및 재임포트
            importer.SaveAndReimport();
            
            Debug.Log($"Settings applied to: {assetPath}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to apply settings to {assetPath}: {e.Message}");
            return false;
        }
    }
}
