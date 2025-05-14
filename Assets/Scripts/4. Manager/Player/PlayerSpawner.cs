using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;

    [SerializeField] private Transform spawnPoint;

    private void OnEnable()
    {
        InstanceFinder.SceneManager.OnLoadEnd += OnLoadEnd;
    }

    private void OnDisable()
    {
        InstanceFinder.SceneManager.OnLoadEnd -= OnLoadEnd;
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
