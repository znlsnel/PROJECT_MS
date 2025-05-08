using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : NetworkBehaviour
{
    [Header("플레이어 설정")]
    [SerializeField] private NetworkObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        InstanceFinder.SceneManager.OnLoadStart += OnScenesLoadedStart;
        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();

        InstanceFinder.SceneManager.OnLoadStart -= OnScenesLoadedStart;
        InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoadEnd;
    }

    private void OnScenesLoadedStart(SceneLoadStartEventArgs args)
    {
        if(!IsClientStarted) return;
    }

    private void OnSceneLoadEnd(SceneLoadEndEventArgs args)
    {
        if(!IsServerStarted) return;

        Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        if (scene.name == "Demo")
        {
            // 모든 클라이언트에 대해 플레이어 생성
            foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
            {
                SpawnPlayerForClient(conn, scene);
            }
        }
    }

    private void SpawnPlayerForClient(NetworkConnection conn, Scene scene)
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
        playerInstance.GetComponent<NetworkTransform>().SetIsNetworked(true);
        InstanceFinder.ServerManager.Spawn(playerInstance, conn, scene);
        
        Debug.Log($"클라이언트 {conn.ClientId}에 대한 플레이어가 생성되었습니다.");
    }
}
