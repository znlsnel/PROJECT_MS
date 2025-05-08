using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using System.Collections.Generic;
using FishNet.Connection;

public class SceneManagerEx : NetworkBehaviour
{
    private HashSet<NetworkConnection> readyClients = new HashSet<NetworkConnection>();

    [Header("씬 이동 설정")]
    [SerializeField] private string[] availableScenes;
    [SerializeField] private string loadingSceneName = "LoadingScene";
    [SerializeField] private bool useLoadingScene = true;

    private bool isInTransition = false;
    public float LoadingProgress { get; private set; }

    private AsyncOperation operation;

    public void Init()
    {
        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoaded;
    }

    public void Clear()
    {
        InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoaded;
    }

    private void OnSceneLoaded(SceneLoadEndEventArgs args)
    {
        for(int i = 0; i < args.LoadedScenes.Length; i++)
        {
            GameObject[] rootObjects = args.LoadedScenes[i].GetRootGameObjects();

            for(int j = 0; j < rootObjects.Length; j++)
            {
                if(rootObjects[j].TryGetComponent(out SceneInitializer sceneInitializer))
                    sceneInitializer.Initialize();
            }
        }
    }

    /// <summary>
    /// 지정된 씬으로 이동합니다.
    /// </summary>
    /// <param name="sceneName">이동할 씬 이름</param>
    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (isInTransition)
            return;

        LoadSceneAsync(sceneName, loadSceneMode).Forget();
    }

    /// <summary>
    /// 지정된 인덱스의 씬으로 이동합니다.
    /// </summary>
    /// <param name="sceneIndex">availableScenes 배열에서의 인덱스</param>
    public void LoadScene(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (isInTransition || sceneIndex < 0 || sceneIndex >= availableScenes.Length)
            return;

        LoadSceneAsync(availableScenes[sceneIndex], loadSceneMode).Forget();
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

        operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        operation.allowSceneActivation = false;

        while (operation.progress >= 0.9f)
        {
            LoadingProgress = operation.progress / 0.9f;
            await UniTask.Yield();
        }

        LoadSceneServerRpc(sceneName);
    }

    [ServerRpc(RequireOwnership = false)]
    private void LoadSceneServerRpc(string sceneName, NetworkConnection conn = null)
    {
        readyClients.Add(conn);

        if(readyClients.Count == InstanceFinder.ServerManager.Clients.Count)
        {
            foreach(var client in InstanceFinder.ServerManager.Clients)
                LoadSceneClientRpc(client.Value, sceneName);
        }
    }

    [TargetRpc]
    private void LoadSceneClientRpc(NetworkConnection conn, string sceneName)
    {
        InstanceFinder.SceneManager.LoadConnectionScenes(conn, new SceneLoadData(sceneName));

        operation.allowSceneActivation = true;

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(loadingSceneName);

        isInTransition = false;
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
