using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;

    [SerializeField] private Transform spawnPoints;

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerRpc(NetworkConnection conn = null)
    {
        NetworkObject player = Instantiate(playerPrefab, spawnPoints.position, spawnPoints.rotation);
        InstanceFinder.ServerManager.Spawn(player, conn);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if(spawnPoints == null)
            return;

        Gizmos.DrawWireSphere(spawnPoints.position, 1f);
    }
}
