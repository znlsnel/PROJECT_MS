using System.Collections;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;

    [SerializeField] private Transform spawnPoint;

    public override void OnStartClient()
    {
        NetworkCommandSystem.Instance.RequestSpawnPlayer(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public override void OnStopClient()
    {
        NetworkCommandSystem.Instance.RequestDespawnPlayer(playerPrefab);
    }

    private void OnLoadEnd(SceneLoadEndEventArgs args)
    {
        NetworkCommandSystem.Instance.RequestSpawnPlayer(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if(spawnPoint == null)
            return;

        Gizmos.DrawWireSphere(spawnPoint.position, 1f);
    }
}
