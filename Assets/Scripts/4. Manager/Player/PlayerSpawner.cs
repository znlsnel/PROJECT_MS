using System.Collections;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [field: SerializeField] public NetworkObject playerPrefab { get; private set;}

    [field: SerializeField] public Transform spawnPoint { get; private set;}

    [field: SerializeField] public bool UseAutoSpawning { get; private set;} = false;

    public override void OnStartClient()
    {
        if(!UseAutoSpawning)
            return;

        NetworkCommandSystem.Instance?.RequestSpawnPlayer(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public override void OnStopClient()
    {
        if(!UseAutoSpawning)
            return;

        NetworkCommandSystem.Instance?.RequestDespawnPlayer(playerPrefab);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if(spawnPoint == null)
            return;

        Gizmos.DrawWireSphere(spawnPoint.position, 1f);
    }
}
