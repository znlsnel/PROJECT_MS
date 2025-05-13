using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class NetworkCommandSystem : NetworkSingleton<NetworkCommandSystem>
{
    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnPlayer(NetworkObject playerPrefab, Vector3 position, Quaternion rotation, NetworkConnection conn = null)
    {
        if(conn.FirstObject != null)
            return;

        NetworkObject playerInstance = Instantiate(playerPrefab, position, rotation);
        InstanceFinder.ServerManager.Spawn(playerInstance, conn);
    }
}
