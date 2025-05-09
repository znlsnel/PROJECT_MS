using FishNet;
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
        NetworkObject player = Instantiate(playerPrefab);
        InstanceFinder.ServerManager.Spawn(player);
    }
}
