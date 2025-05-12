using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class DemeSceneInitializer : SceneInitializer
{
    [SerializeField] private NetworkObject playerPrefab;

    public override void Initialize()
    {
        SpawnPlayer();
    }

    [Server]
    public void SpawnPlayer()
    {
        foreach(var conn in InstanceFinder.ServerManager.Clients)
        {
            NetworkObject player = Instantiate(playerPrefab);
            InstanceFinder.ServerManager.Spawn(player, conn.Value);
        }
    }
}
