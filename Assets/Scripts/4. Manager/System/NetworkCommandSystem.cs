using FishNet;
using FishNet.Connection;
using FishNet.Object;
using Steamworks;
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

    [ServerRpc(RequireOwnership = false)]
    public void RequestDespawnPlayer(NetworkObject playerPrefab, NetworkConnection conn = null)
    {
        if(conn.FirstObject == null)
            return;

        InstanceFinder.ServerManager.Despawn(playerPrefab);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnObject(NetworkObject objectPrefab, Vector3 position, Quaternion rotation, NetworkConnection conn = null)
    {
        NetworkObject instance = Instantiate(objectPrefab, position, rotation);
        InstanceFinder.ServerManager.Spawn(instance, conn);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestDespawnObject(NetworkObject objectPrefab, NetworkConnection conn = null)
    {
        InstanceFinder.ServerManager.Despawn(objectPrefab);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestDropItem(NetworkObject itemPrefab, Vector3 position, Quaternion rotation, int durability, NetworkConnection conn = null)
    {
        NetworkObject instance = Instantiate(itemPrefab, position, rotation);
        instance.GetComponent<ItemObject>().SetDurability(durability); 
        InstanceFinder.ServerManager.Spawn(instance, conn);


        instance.gameObject.GetOrAddComponent<Rigidbody>(); 
    }

}