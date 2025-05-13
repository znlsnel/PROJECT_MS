using FishNet;
using Steamworks;
using UnityEngine;

public class SteamManagerEx : IManager
{
    private Callback<LobbyCreated_t> _lobbyCreated;
    private Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
    private Callback<LobbyEnter_t> _lobbyEntered;
    private Callback<LobbyMatchList_t> _lobbyList;
    
    public ELobbyType LobbyType = ELobbyType.k_ELobbyTypePublic;

    public FishySteamworks.FishySteamworks FishySteamworks { get; private set; }

    public static ulong CurrentLobbyId { get; private set; } = 0;

    private const string HostAddressKey = "HostAddress";

    public void Init()
    {
        if(Managers.Network.Type != NetworkType.Steam) return;

        FishySteamworks = Object.FindAnyObjectByType<FishySteamworks.FishySteamworks>();

        SteamAPI.Init();

        RegisterCallbacks();
    }

    public void Update()
    {
        if(SteamAPI.IsSteamRunning())
            SteamAPI.RunCallbacks();
    }

    public void Clear()
    {
        if(SteamAPI.IsSteamRunning())
            SteamAPI.Shutdown();
    }

    private void RegisterCallbacks()
    {
        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        _lobbyList = Callback<LobbyMatchList_t>.Create(OnLobbyList);
    }

    public void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(LobbyType, Managers.Network.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("Lobby 생성 실패");
            return;
        }

        CurrentLobbyId = callback.m_ulSteamIDLobby;
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyId), HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyId), "name", SteamFriends.GetPersonaName().ToString() + "'s lobby");
        FishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        FishySteamworks.StartConnection(true);
        Debug.Log("Lobby creation was successful");
    }

    private void OnJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyId = callback.m_ulSteamIDLobby;

        FishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyId), HostAddressKey));
        FishySteamworks.StartConnection(false);
    }

    public void JoinByID()
    {
        JoinByID(CurrentLobbyId);
    }

    public void JoinByID(ulong steamID)
    {
        Debug.Log("Attempting to join lobby with ID: " + steamID);

        if(SteamMatchmaking.RequestLobbyData(new CSteamID(steamID)))
            SteamMatchmaking.JoinLobby(new CSteamID(steamID));
        else
            Debug.LogError("Failed to join lobby with ID: " + steamID);
    }

    public void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyId));
        CurrentLobbyId = 0;

        FishySteamworks.StopConnection(false);
        if(InstanceFinder.NetworkManager.IsServerStarted)
            FishySteamworks.StopConnection(true);
    }

    private void OnLobbyList(LobbyMatchList_t callback)
    {
        
    }

    public void StartGame()
    {
        string[] scenesToClose = new string[] { "" };
    }
}
