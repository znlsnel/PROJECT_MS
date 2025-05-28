
using System;

using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkGameSystem : NetworkSingleton<NetworkGameSystem>
{
    public readonly SyncVar<bool> IsGameStarted = new SyncVar<bool>(false);
    public readonly SyncVar<GameOptions> GameOptions = new SyncVar<GameOptions>(new GameOptions(1, 300, 3));
    public readonly SyncDictionary<NetworkConnection, PlayerInfo> Players = new SyncDictionary<NetworkConnection, PlayerInfo>();
    public readonly SyncList<NetworkConnection> Imposters = new SyncList<NetworkConnection>();
    [SerializeField] private NetworkObject ghostPlayerPrefab;
    private List<NetworkObject> ghostPlayers = new List<NetworkObject>();

    public static Action onGameStart;
    public static Action onGameEnd;

    private static readonly string winSound = "Sound/WinLose/Win.mp3";
    private static readonly string loseSound = "Sound/WinLose/Lose_01.mp3";

    [Server]
    public void StartGame()
    {
        IsGameStarted.Value = true;
        onGameStart?.Invoke();
        
        SetRandomRole();

        NetworkSceneSystem.Instance?.LoadScene("Game");
    }

    [Server]
    public void EndGame(EPlayerRole winner)
    {
        IsGameStarted.Value = false;
        EndGame_InClient(winner); 
    } 

    [ObserversRpc]
    public void EndGame_InClient(EPlayerRole winner)
    {
        onGameEnd?.Invoke();

        EPlayerRole currentPlayerRole = GetPlayerRole(InstanceFinder.ClientManager.Connection);

        if ((winner == EPlayerRole.Imposter && currentPlayerRole == EPlayerRole.Imposter) ||
            (winner == EPlayerRole.Survival && currentPlayerRole == EPlayerRole.Survival))
        {
            Managers.Sound.Play(winSound);
        }
        else
        {
            Managers.Sound.Play(loseSound);
        }
    }

    [Server]
    public void SetGameOptions(GameOptions options)
    {
        GameOptions.Value = options;
    }

    [Server]
    private void SetRandomRole()
    {
        Players.Clear();
        Imposters.Clear();

        
        Imposters.AddRange(InstanceFinder.ServerManager.Clients.Values
            .OrderBy(x => Random.value)
            .Take(GameOptions.Value.imposterCount)
            .ToList());

        foreach(NetworkConnection connection in InstanceFinder.ServerManager.Clients.Values)
        {
            EPlayerRole role = Imposters.Contains(connection) ? EPlayerRole.Imposter : EPlayerRole.Survival;
            PlayerInfo playerInfo = new PlayerInfo(connection.ClientId.ToString(), role, false, 0);
            Debug.Log(playerInfo);
            Players.Add(connection, playerInfo);
        }
    }

    public EPlayerRole GetPlayerRole(NetworkConnection connection)
    {
        if(Players.TryGetValue(connection, out PlayerInfo playerInfo))
        {
            return playerInfo.role;
        }
        return EPlayerRole.Survival;
    }

    public void ImposterWin()
    {
        Debug.Log("Imposter Win");
        EndGame(EPlayerRole.Imposter);
    }

    public void SurvivalWin()
    {
        Debug.Log("Survival Win");
        EndGame(EPlayerRole.Survival);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnPlayerDead(NetworkObject networkObject, NetworkConnection connection = null)
    {
        if(Players.TryGetValue(connection, out PlayerInfo playerInfo))
        {
            playerInfo.isDead = true;
            Players[connection] = playerInfo;
        }

        int aliveSurvivals = Players.Count(player => player.Value.role == EPlayerRole.Survival && !player.Value.isDead);

        foreach(PlayerInfo info in Players.Values)
        {
            NetworkChatSystem.Instance.SendChatMessage(info.isDead ? "You are dead" : "You are alive");
        }

         if(aliveSurvivals <= 0)
        {
            Managers.Analytics.MafiaWinRate(true);
            ImposterWin();
        }
        else
        {
            // NetworkObject instance = Instantiate(ghostPlayerPrefab, position, Quaternion.identity);
            // InstanceFinder.ServerManager.Spawn(instance, connection);
        }

        NetworkObject instance = Instantiate(ghostPlayerPrefab, networkObject.transform.position, Quaternion.identity);
        InstanceFinder.ServerManager.Spawn(instance, connection);
        ghostPlayers.Add(instance);
    }

    [Server] 
    public void UpdatePlayerKillCount(NetworkConnection connection = null)
    {
        if(Players.TryGetValue(connection, out PlayerInfo playerInfo))
        {
            playerInfo.killCount++;
            Players[connection] = playerInfo;
        }
    }
}

public struct GameOptions
{
    public int imposterCount;
    /// <summary>
    /// expression in seconds
    /// </summary>
    public int dayDuration;
    public int maxDayCount;

    public GameOptions(int imposterCount, int dayDuration, int maxDayCount)
    {
        this.imposterCount = imposterCount;
        this.dayDuration = dayDuration;
        this.maxDayCount = maxDayCount;
    }
}

public struct PlayerInfo
{
    public string playerName;
    public int killCount; 
    public EPlayerRole role;
    public bool isDead;

    public PlayerInfo(string playerName, EPlayerRole role, bool isDead, int killCount)
    {
        this.playerName = playerName;
        this.role = role;
        this.isDead = isDead;
        this.killCount = killCount;
    }
}

public enum EPlayerRole
{
    Survival,
    Imposter,
}