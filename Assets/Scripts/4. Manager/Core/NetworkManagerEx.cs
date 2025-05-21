using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using Steamworks;
using UnityEngine;

[System.Serializable]
public class NetworkManagerEx : IManager
{
    [field: SerializeField] public int maxConnections { get; set; } = 10;
    [field: SerializeField] public string networkAddress { get; set; } = "127.0.0.1";

    [SerializeField] private NetworkBehaviour[] NetworkPrefabs;

    public List<NetworkBehaviour> NetworkSystems { get; private set; } = new List<NetworkBehaviour>();

    [field: Header("Network Type")]
    [field: SerializeField] public NetworkType Type { get; private set; } = NetworkType.TCP_UDP;

    [SerializeField] private NetworkManager networkManagerTCP_UDP;
    [SerializeField] private NetworkManager networkManagerSteam;


    public void Init()
    {
        InstanceFinder.ServerManager.OnServerConnectionState += OnServerConnectionState;
    }

    public void Clear()
    {
        InstanceFinder.ServerManager.OnServerConnectionState -= OnServerConnectionState;
    }

    public void OnValidate()
    {
        if (networkManagerTCP_UDP == null || networkManagerSteam == null)
            return;
            
        if(Type == NetworkType.TCP_UDP)
        {
            networkManagerTCP_UDP.gameObject.SetActive(true);
            networkManagerSteam.gameObject.SetActive(false);
        }
        else
        {
            networkManagerTCP_UDP.gameObject.SetActive(false);
            networkManagerSteam.gameObject.SetActive(true);
        }
    }

    private void OnServerConnectionState(ServerConnectionStateArgs args)
    {
        switch(args.ConnectionState)
        {
            case LocalConnectionState.Started:
                foreach(NetworkBehaviour prefab in NetworkPrefabs)
                {
                    NetworkBehaviour networkSystem = Object.Instantiate(prefab);
                    networkSystem.NetworkObject.SetIsGlobal(true);
                    InstanceFinder.ServerManager.Spawn(networkSystem.NetworkObject);
                    NetworkSystems.Add(networkSystem);
                }
                break;
            case LocalConnectionState.Stopped:
                foreach(NetworkBehaviour networkSystem in NetworkSystems)
                {
                    InstanceFinder.ServerManager.Despawn(networkSystem.NetworkObject);
                }
                NetworkSystems.Clear();
                break;
        }
    }

    public void StartServer()
    {
        if(Type == NetworkType.TCP_UDP)
        {
            InstanceFinder.ServerManager.StartConnection();
        }
        else if(Type == NetworkType.Steam)
        {
            Managers.Steam.CreateLobby();
        }
    }
    
    public bool StartClient(string address = "localhost")
    {
        if(Type == NetworkType.TCP_UDP)
        {
            return InstanceFinder.ClientManager.StartConnection(address);
        }
        else if(Type == NetworkType.Steam)
        {
            return Managers.Steam.JoinByID(ulong.Parse(address));
        }

        return false;
    }

    public void StartHost()
    {
        if(Type == NetworkType.TCP_UDP)
        {
            StartServer();
            StartClient();
        }
        else if(Type == NetworkType.Steam)
        {
            Managers.Steam.CreateLobby();
        }
    }

    public void StopServer()
    {
        if(Type == NetworkType.TCP_UDP)
        {
            InstanceFinder.ServerManager.StopConnection(true);
        }
        else if(Type == NetworkType.Steam)
        {
            Managers.Steam.LeaveLobby();
        }
    }

    public void StopClient()
    {
        if(Type == NetworkType.TCP_UDP)
        {
            InstanceFinder.ClientManager.StopConnection();
        }
        else if(Type == NetworkType.Steam)
        {
            Managers.Steam.LeaveLobby();
        }
    }

    public void StopHost()
    {
        if(Type == NetworkType.TCP_UDP)
        {
            StopClient();
            StopServer();
        }
        else if(Type == NetworkType.Steam)
        {
            Managers.Steam.LeaveLobby();
        }
    }
}
