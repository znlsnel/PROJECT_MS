using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class GameSceneManager : NetworkBehaviour
{
    [Header("플레이어 설정")]
    [SerializeField] private NetworkObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private Dictionary<int, NetworkObject> _spawnedPlayers = new Dictionary<int, NetworkObject>();

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        if(!IsServerStarted) return;

        InstanceFinder.SceneManager.OnClientLoadedStartScenes += OnClientLoadedStartScenes;
        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();

        if(!IsServerStarted) return;

        InstanceFinder.SceneManager.OnClientLoadedStartScenes -= OnClientLoadedStartScenes;
        InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoadEnd;
    }

    private void OnClientLoadedStartScenes(NetworkConnection conn, bool isFirstLoad)
    {
        if(!IsServerStarted) return;

        if(_spawnedPlayers.ContainsKey(conn.ClientId)) return;

        //SpawnPlayerForClient(conn);
    }

    private void OnSceneLoadEnd(SceneLoadEndEventArgs args)
    {
        if(!IsServerStarted) return;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Demo")
        {
            // 모든 클라이언트에 대해 플레이어 생성
            foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
            {
                if (!_spawnedPlayers.ContainsKey(conn.ClientId))
                {
                    SpawnPlayerForClient(conn);
                }
            }
        }
    }

    private void SpawnPlayerForClient(NetworkConnection conn)
    {
        // 스폰 위치 결정
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;
        
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int spawnIndex = conn.ClientId % spawnPoints.Length;
            Transform spawnPoint = spawnPoints[spawnIndex];
            spawnPosition = spawnPoint.position;
            spawnRotation = spawnPoint.rotation;
        }
        
        // 플레이어 생성
        NetworkObject playerInstance = Instantiate(playerPrefab, spawnPosition, spawnRotation);
        InstanceFinder.ServerManager.Spawn(playerInstance, conn, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        
        // 생성된 플레이어 저장
        _spawnedPlayers[conn.ClientId] = playerInstance;
        
        Debug.Log($"클라이언트 {conn.ClientId}에 대한 플레이어가 생성되었습니다.");
    }
}
