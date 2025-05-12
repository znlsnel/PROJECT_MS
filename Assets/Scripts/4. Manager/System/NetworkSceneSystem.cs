using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using System.Collections;

public class NetworkSceneSystem : NetworkSingleton<NetworkSceneSystem>
{
    private HashSet<NetworkConnection> readyClients = new HashSet<NetworkConnection>();

    [Header("씬 이동 설정")]
    [SerializeField] private string[] availableScenes;
    [SerializeField] private string loadingSceneName = "LoadingScene";
    [SerializeField] private bool useLoadingScene = true;

    private bool isInTransition = false;

    public string CurrentTaskName { get; private set; }
    public float CurrentTaskProgress { get; private set; }

    public static event System.Action OnLoadingStart;
    public static event System.Action OnLoadingEnd;
    public static event System.Action<string> OnChangeTaskName;
    public static event System.Action<float> OnChangeTaskProgress;

    public void OnEnable()
    {
        InstanceFinder.SceneManager.OnLoadPercentChange += OnLoadPercentChange;
        InstanceFinder.SceneManager.OnLoadEnd += OnLoadEnd;
    }

    public void OnDisable()
    {
        InstanceFinder.SceneManager.OnLoadPercentChange -= OnLoadPercentChange;
        InstanceFinder.SceneManager.OnLoadEnd -= OnLoadEnd;
    }

    /// <summary>
    /// 지정된 씬으로 이동합니다.
    /// </summary>
    /// <param name="sceneName">이동할 씬 이름</param>
    [Server]
    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (isInTransition)
            return;

        LoadSceneRpc(sceneName, loadSceneMode);
    }

    /// <summary>
    /// 지정된 인덱스의 씬으로 이동합니다.
    /// </summary>
    /// <param name="sceneIndex">availableScenes 배열에서의 인덱스</param>
    [Server]
    public void LoadScene(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (isInTransition || sceneIndex < 0 || sceneIndex >= availableScenes.Length)
            return;

        LoadSceneRpc(availableScenes[sceneIndex], loadSceneMode);
    }

    [ObserversRpc]
    private void LoadSceneRpc(string sceneName, LoadSceneMode loadSceneMode)
    {
        LoadSceneAsync(sceneName, loadSceneMode).Forget();
    }

    private async UniTaskVoid LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
    {
        isInTransition = true;

        CurrentTaskProgress = 0f;
        OnLoadingStart?.Invoke();

        CurrentTaskName = "게임 데이터 불러오는 중...";
        OnChangeTaskName?.Invoke(CurrentTaskName);

        Managers.Resource.LoadAllAsync<Object>("default", (key, count, total) =>
        {
            CurrentTaskProgress = (float)count / total;
            OnChangeTaskProgress?.Invoke(CurrentTaskProgress);
        });
        
        while(CurrentTaskProgress < 1f)
        {
            await UniTask.Yield();
        }

        await UniTask.WaitForEndOfFrame();

        CurrentTaskName = "씬 로드 중...";
        OnChangeTaskName?.Invoke(CurrentTaskName);

        OnPreloadComplete(sceneName);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnPreloadComplete(string sceneName, NetworkConnection conn = null)
    {
        readyClients.Add(conn);

        if(readyClients.Count == InstanceFinder.ServerManager.Clients.Count)
        {
            SceneLoadData sceneLoadData = new SceneLoadData(sceneName);
            sceneLoadData.ReplaceScenes = ReplaceOption.All;

            InstanceFinder.SceneManager.LoadGlobalScenes(sceneLoadData);
        }
    }

    private void OnLoadPercentChange(SceneLoadPercentEventArgs args)
    {
        CurrentTaskProgress = args.Percent;
        OnChangeTaskProgress?.Invoke(CurrentTaskProgress);
    }

    private void OnLoadEnd(SceneLoadEndEventArgs args)
    {
        isInTransition = false;
        OnLoadingEnd?.Invoke();
        SceneInitializer();
    }

    private void SceneInitializer()
    {
        NetworkSceneInitializer sceneInitializer = Object.FindAnyObjectByType<NetworkSceneInitializer>();
        sceneInitializer?.Initialize();
    }

    /// <summary>
    /// 지정된 씬을 언로드합니다.
    /// </summary>
    /// <param name="sceneName">언로드할 씬 이름</param>
    public void UnloadScene(string sceneName)
    {
        UnloadSceneAsync(sceneName).Forget();
    }

    private async UniTaskVoid UnloadSceneAsync(string sceneName)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        
        while (operation != null && !operation.isDone)
        {
            await UniTask.Yield();
        }
    }
}
