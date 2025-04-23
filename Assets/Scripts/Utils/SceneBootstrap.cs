
using UnityEngine;

public class SceneBootstrap
{
    public static string StartingSceneName { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Bootstrap()
    {
        if(string.IsNullOrEmpty(StartingSceneName))
        {
            StartingSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        else
        {
            Debug.Log($"Starting Scene Already Set: {StartingSceneName}");
        }

        if(StartingSceneName != "TitleScene")
        {
            Debug.Log("Non-Title Scene Detected. Forcing Initialization...");
            
            InitializeManagers();
        }
        else
        {
            Debug.Log("Title Scene Detected. No Forced Initialization Required.");
        }
    }

    private static void InitializeManagers()
    {
        if(Managers.Instance == null)
        {
            CreateManager("GameManager", "Manager/GameManager");
        }
    }

    private static void CreateManager(string managerName, string resourcePath)
    {
        GameObject prefab = Resources.Load<GameObject>(resourcePath);

        if(prefab == null)
        {
          //  Debug.LogError($"SceneBootstrapper: {managerName} not found prefab at {resourcePath} path");
            return; 
        }

        GameObject instance = Object.Instantiate(prefab);
        instance.name = managerName;
        Object.DontDestroyOnLoad(instance);

        Debug.Log($"SceneBootstrapper: {managerName} dynamically created");
    }
}
