using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class DemeSceneInitializer : NetworkSceneInitializer
{
    [SerializeField] private NetworkObject playerPrefab;

    public override void Initialize()
    {
        SpawnPlayer(Owner);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayer(NetworkConnection conn = null)
    {
        NetworkObject player = Instantiate(playerPrefab);
        InstanceFinder.ServerManager.Spawn(player, conn);
    }
}
