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
    public bool IsGameStarted = false;
    public readonly SyncVar<GameOptions> GameOptions = new SyncVar<GameOptions>(new GameOptions(1, 300, 3));  
    public readonly SyncDictionary<NetworkConnection, PlayerInfo> Players = new SyncDictionary<NetworkConnection, PlayerInfo>();
    public readonly SyncList<NetworkConnection> Imposters = new SyncList<NetworkConnection>();
    [SerializeField] private NetworkObject ghostPlayerPrefab;
    private List<NetworkObject> ghostPlayers = new List<NetworkObject>();

    public static Action onGameStart;
    public static Action<EPlayerRole> onGameEnd;
    public Action onCompletedRandomRole;
    public bool completedRandomRole = false;

    private static readonly string winSound = "Sound/WinLose/Win.mp3";
    private static readonly string loseSound = "Sound/WinLose/Lose_01.mp3";

    [Server]
    public void StartGame()
    {
        IsGameStarted = true;
        onGameStart?.Invoke();
        
        if(Managers.Network.Type == NetworkType.Steam)
        {
            Managers.Steam.SetLobbyInvisible();
        }
        
        SetRandomRole();

        Players.OnChange += OnPlayerChange;

        NetworkSceneSystem.Instance?.LoadScene("Game");
    }

    private void OnPlayerChange(SyncDictionaryOperation op, NetworkConnection key, PlayerInfo value, bool asServer)
    {
        if(!asServer || !IsGameStarted) return;

        int aliveSurvivals = Players.Count(player => player.Value.role == EPlayerRole.Survival && !player.Value.isDead);

        if(aliveSurvivals <= 0)
        {
            Managers.Analytics.MafiaWinRate(true);
            ImposterWin();
        }
        else
        {
            NetworkObject instance = Instantiate(ghostPlayerPrefab, key.FirstObject.transform.position, Quaternion.identity);
            InstanceFinder.ServerManager.Spawn(instance, key);
            ghostPlayers.Add(instance);
        }
    }

    [Server]
    public void EndGame(EPlayerRole winner)
    {
        IsGameStarted = false;
        Players.OnChange -= OnPlayerChange;
        
        // 게임 종료 시 SteamLobby를 다시 보이게 설정 (서버에서만 실행)
        if(Managers.Network.Type == NetworkType.Steam)
        {
            Managers.Steam.SetLobbyVisible();
        }
        
        EndGame_InClient(winner); 
    } 

    [ObserversRpc]
    public void EndGame_InClient(EPlayerRole winner)
    {
        onGameEnd?.Invoke(winner);

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
            Players.Add(connection, playerInfo);
        }
        CompletedRandomRole(); 
    }

    [ObserversRpc]
    public void CompletedRandomRole() 
    {
        completedRandomRole = true; 
        onCompletedRandomRole?.Invoke(); 
    }

    public void SubscribeToRandomRole(Action callback)
    {
        if (completedRandomRole)
            callback?.Invoke();
        else
            onCompletedRandomRole += callback; 
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
        EndGame(EPlayerRole.Imposter);
    }

    public void SurvivalWin()
    {
        EndGame(EPlayerRole.Survival);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnPlayerDead(NetworkConnection connection = null)
    {
        if(Players.TryGetValue(connection, out PlayerInfo playerInfo))
        {
            playerInfo.isDead = true;
            Players[connection] = playerInfo;
        }
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