using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class DemeSceneInitializer : NetworkSceneInitializer
{
    [SerializeField] private NetworkObject playerPrefab;

    public override void Initialize()
    {
        RequestSpawnPlayer();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnPlayer(NetworkConnection conn = null)
    {
        if(conn.FirstObject != null)
            return;

        NetworkSceneSystem.Instance?.SpawnPlayerForConnection(conn, playerPrefab);
    }
}
