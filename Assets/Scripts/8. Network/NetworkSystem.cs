using System;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class NetworkSystem : NetworkBehaviour
{
    private static NetworkSystem instance;
    public static NetworkSystem Instance => instance;

    public void Awake()
    {
        instance = this;
    }

    public void OnStartGameButtonClicked()
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
}
