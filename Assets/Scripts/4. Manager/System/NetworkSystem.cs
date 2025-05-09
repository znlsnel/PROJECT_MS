using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSystem : NetworkBehaviour
{
    private static NetworkSystem instance;
    public static NetworkSystem Instance => instance;

    [Header("씬 이동 설정")]
    [SerializeField] private string loadingSceneName = "LoadingScene";
    [SerializeField] private bool useLoadingScene = true;

    private bool isInTransition = false;
    public float LoadingProgress { get; private set; }

    public void Awake()
    {
        instance = this;
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        SceneManager.OnLoadStart += OnSceneLoadingStart;
        SceneManager.OnLoadPercentChange += OnSceneLoading;
        SceneManager.OnLoadEnd += OnSceneLoaded;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();

        SceneManager.OnLoadStart -= OnSceneLoadingStart;
        SceneManager.OnLoadPercentChange -= OnSceneLoading;
        SceneManager.OnLoadEnd -= OnSceneLoaded;
    }

    public void OnGameStartButtonClicked()
    {
        if(IsClientOnlyStarted)
            ServerStartGame();

        else if (IsServerStarted)
            StartGame();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerStartGame()
    {
        StartGame();
    }

    public void LoadGlobalScenes(string targetSceneName, string loadingSceneName)
    {
        SceneLoadData targetSceneData = new SceneLoadData(targetSceneName);
        SceneLoadData loadingSceneData = new SceneLoadData(loadingSceneName);
        InstanceFinder.SceneManager.LoadGlobalScenes(loadingSceneData);
    }

    [Server]
    private void StartGame()
    {
        SceneLoadData sld = new SceneLoadData("Demo");
        SceneUnloadData sud = new SceneUnloadData("Lobby");
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
        InstanceFinder.SceneManager.UnloadGlobalScenes(sud);
    }

    public static void ChangeNetworkScene(string sceneName, string[] scenesToClose)
    {
        instance.CloseScenes(scenesToClose);

        SceneLoadData sld = new SceneLoadData(sceneName);
        NetworkConnection[] conns = instance.ServerManager.Clients.Values.ToArray();
        instance.SceneManager.LoadConnectionScenes(conns, sld);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void CloseScenes(string[] scenesToClose)
    {
        CloseScenesObservers(scenesToClose);
    }

    [ObserversRpc]
    private void CloseScenesObservers(string[] scenesToClose)
    {
        foreach(string sceneName in scenesToClose)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    private void OnSceneLoadingStart(SceneLoadStartEventArgs args)
    {
        isInTransition = true;
        LoadingProgress = 0f;

        // 로딩 씬 로드
        if(useLoadingScene)
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);
    }

    private void OnSceneLoading(SceneLoadPercentEventArgs args)
    {
        LoadingProgress = args.Percent;
    }

    private void OnSceneLoaded(SceneLoadEndEventArgs args)
    {
        if(useLoadingScene)
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(loadingSceneName);
        
        isInTransition = false;

        GameObject.FindAnyObjectByType<SceneInitializer>()?.Initialize();
    }

    private async UniTaskVoid LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
    {
        isInTransition = true;
        LoadingProgress = 0f;

        // 로딩 씬 로드
        if(useLoadingScene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);
        }

        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            LoadingProgress = operation.progress / 0.9f;
            if(operation.progress >= 0.9f)
                operation.allowSceneActivation = true;
            
            await UniTask.Yield();
        }
        await UniTask.WaitUntil(() => operation.isDone);

        await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(loadingSceneName);

        isInTransition = false;
    }
}
