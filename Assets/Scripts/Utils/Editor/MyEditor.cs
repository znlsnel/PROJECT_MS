using UnityEngine;
using UnityEditor;
using System.Collections;
using Hamster.ZG.Type;
using UGS.Editor;
using System.Linq;
using FishNet.Managing.Object;
using FishNet;

public class MyEditor : EditorWindow
{
    private string resetMessage = "";
    private double resetMessageTime = 0f;

    [MenuItem("CustomEditor/Item Editor")]
    public static void ShowWindow()
    {
        GetWindow<MyEditor>("Item Editor");
    }

    void OnGUI()
    {
        DrawMainButtons();
        GUILayout.FlexibleSpace(); // 남은 공간을 모두 차지하여 메시지가 최하단에 위치하도록 함
        GUILayout.Space(20); // 최하단에서 20px 띄움
        DrawResetMessage();
    }

    private void DrawMainButtons()
    {
        if (GUILayout.Button("드롭 아이템 데이터 초기화", GUILayout.Height(30)))
        {
            DropItemInit();
            ShowResetMessage();
        }

        if (GUILayout.Button("아이템 컴포넌트 자동 세팅", GUILayout.Height(30)))
        {
            ItemInit();
            ShowResetMessage();
        }


        if (GUILayout.Button("자원 데이터 초기화", GUILayout.Height(30)))
        {
            ResourceInit();
            ShowResetMessage();
        }

        if (GUILayout.Button("메시 설정 배치 처리", GUILayout.Height(30))) 
        {
            MeshSerttingsBatchProcessor.ShowWindow();
            ShowResetMessage();
        }


        EditorGUILayout.Space(10);

        if (GUILayout.Button("아이템 만들기", GUILayout.Height(30)))
        {
            // 아이템 만들기 로직 실행
            ItemCreateWindow.ShowWindow();
        }
    }

    private void ShowResetMessage()
    {
        resetMessage = "초기화가 완료되었습니다.";
        resetMessageTime = EditorApplication.timeSinceStartup;
        Repaint();
        AssetDatabase.SaveAssets(); 
    }

    private void DrawResetMessage()
    {
        if (!string.IsNullOrEmpty(resetMessage))
        {
            // 3초가 지났는지 확인
            if (EditorApplication.timeSinceStartup - resetMessageTime > 2.0)
            {
                resetMessage = "";
                Repaint();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
      
                EditorGUILayout.HelpBox(resetMessage, MessageType.Info);
 
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    private void DropItemInit()
    {

        GameData.Item.Load(true);

        string rootPath = "Assets/2. AddressableAssets/DropItem";
        // 모든 하위 폴더까지 포함하여 에셋 경로 가져오기
        string[] guids = AssetDatabase.FindAssets("", new[] { rootPath });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            // 폴더라면 건너뛴다
            if (AssetDatabase.IsValidFolder(assetPath))
                continue;
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            
            if (asset == null)
                continue;

            string dataPath = assetPath.Replace("Assets/2. AddressableAssets/", "");
            // 필요하다면 리스트에 저장하거나 추가 작업 수행 가능

            GameData.Item item = GameData.Item.GetList().Find(item => item.dropPrefab == dataPath);
            if (item != null)
            {
                Debug.Log($"[초기화 완료] {item.index}, {item.name}");

                ItemObject itemObject = asset.GetOrAddComponent<ItemObject>();
                itemObject.itemId = item.index;
                EditorUtility.SetDirty(asset);
                PrefabUtility.SavePrefabAsset(asset);
            }
        }
        AssetDatabase.SaveAssets();
    }

    private void ItemInit()
    {
        string rootPath = "Assets/2. AddressableAssets/Item";
        // 모든 하위 폴더까지 포함하여 에셋 경로 가져오기
        string[] guids = AssetDatabase.FindAssets("", new[] { rootPath });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            // 폴더라면 건너뛴다
            if (AssetDatabase.IsValidFolder(assetPath))
                continue;

            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (asset == null)
                continue;

            if (assetPath.Contains("Consumable"))
            {
                asset.GetOrAddComponent<ConsumableItemController>();
            }
            else if (assetPath.Contains("Equipment"))
            {
                asset.GetOrAddComponent<ResourceItemController>();
            }
            else if (assetPath.Contains("Resource"))
            {
                asset.GetOrAddComponent<ResourceItemController>();
            }
            else if (assetPath.Contains("Weapon"))
            {
                asset.GetOrAddComponent<WeaponController>();
            }  
            else if (assetPath.Contains("Building"))
            {
                asset.GetOrAddComponent<BuildingItemController>();
            }
            EditorUtility.SetDirty(asset);
            PrefabUtility.SavePrefabAsset(asset);
        }
        AssetDatabase.SaveAssets();

    }

    private void ResourceInit()
    {
        GameData.FieldResource.Load(true);

        string rootPath = "Assets/2. AddressableAssets/FieldResource";
        string[] guids = AssetDatabase.FindAssets("", new[] { rootPath });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            // 폴더라면 건너뛴다
            if (AssetDatabase.IsValidFolder(assetPath))
                continue;
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (asset == null)
                continue;

            string dataPath = assetPath.Replace("Assets/2. AddressableAssets/", "");
            // 필요하다면 리스트에 저장하거나 추가 작업 수행 가능

            GameData.FieldResource fieldResource = GameData.FieldResource.GetList().Find(fieldResource => fieldResource.prefab == dataPath);
            if (fieldResource != null)
            {   
                ResourceHandler resourceHandler = asset.GetOrAddComponent<ResourceHandler>();
                resourceHandler.dropItemIds = fieldResource.dropItems;
                Debug.Log($"[초기화 완료] {fieldResource.index}, {dataPath}"); 

                EditorUtility.SetDirty(asset);
                PrefabUtility.SavePrefabAsset(asset);  
            }
        }
        AssetDatabase.SaveAssets();

    }

}



