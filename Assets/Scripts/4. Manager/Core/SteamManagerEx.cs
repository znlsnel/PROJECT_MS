using System.Collections.Generic;
using System.Threading.Tasks;
using FishNet;
using FishNet.Connection;
using FishNet.Transporting;
using Steamworks;
using Unity.VisualScripting;
using UnityEngine;

public class SteamManagerEx : IManager
{
    private TaskCompletionSource<List<LobbyInfo>> _completionSource;

    private Callback<LobbyCreated_t> _lobbyCreated;
    private Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
    private Callback<LobbyEnter_t> _lobbyEntered;
    private Callback<LobbyMatchList_t> _lobbyList;
    
    public ELobbyType LobbyType = ELobbyType.k_ELobbyTypePublic;

    public FishySteamworks.FishySteamworks FishySteamworks { get; private set; }

    public ulong CurrentLobbyId { get; private set; } = 0;
    public ulong SelectedLobbyId { get; set; } = 0;
    public LobbyInfo lobbyInfo = new LobbyInfo();

    private const string HostAddressKey = "HostAddress";

    public Dictionary<NetworkConnection, ulong> NetworkConnectionToSteamId = new Dictionary<NetworkConnection, ulong>();

    public void Init()
    {
        if(Managers.Network.Type != NetworkType.Steam) return;

        FishySteamworks = Object.FindAnyObjectByType<FishySteamworks.FishySteamworks>();

        SteamAPI.Init();

        RegisterCallbacks();

        InstanceFinder.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
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

        InstanceFinder.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
    }

    private void OnRemoteConnectionState(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        switch(args.ConnectionState)
        {
            case RemoteConnectionState.Started:
                NetworkConnectionToSteamId.Add(connection, SteamUser.GetSteamID().m_SteamID);
                break;
            case RemoteConnectionState.Stopped:
                NetworkConnectionToSteamId.Remove(connection);
                break;
        }
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

        string roomName = lobbyInfo.RoomName.IsUnityNull() ? SteamFriends.GetPersonaName().ToString() + "'s lobby" : lobbyInfo.RoomName;
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyId), "name", roomName);

        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyId), "tag", "ProjectMS");
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

    public bool JoinByID(ulong steamID)
    {
        Debug.Log("Attempting to join lobby with ID: " + steamID);

        if(SteamMatchmaking.RequestLobbyData(new CSteamID(steamID)))
        {
            SteamMatchmaking.JoinLobby(new CSteamID(steamID));
            return true;
        }
        else
        {
            Debug.LogError("Failed to join lobby with ID: " + steamID);
            return false;
        }
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

    public async Task<List<LobbyInfo>> RequestLobbyListAsync()
    {
        _completionSource = new TaskCompletionSource<List<LobbyInfo>>();

        SteamMatchmaking.AddRequestLobbyListStringFilter("tag", "ProjectMS", ELobbyComparison.k_ELobbyComparisonEqual);

        SteamAPICall_t handle = SteamMatchmaking.RequestLobbyList();
        var callResult = CallResult<LobbyMatchList_t>.Create(OnLobbyListReceived);
        callResult.Set(handle);

        return await _completionSource.Task;
    }

    private void OnLobbyListReceived(LobbyMatchList_t result, bool failure)
    {
        if(failure || result.m_nLobbiesMatching == 0)
        {
            Debug.LogError("Lobby 목록 요청 실패");
            _completionSource.SetResult(null);
            return;
        }

        List<LobbyInfo> lobbies = new List<LobbyInfo>();

        Debug.Log("Lobby 목록 요청 성공");

        for(int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyid = SteamMatchmaking.GetLobbyByIndex(i);

            LobbyInfo lobbyInfo = new LobbyInfo();
            lobbyInfo.SetInfo(lobbyid);

            lobbies.Add(lobbyInfo);

            Debug.Log($"Lobby: {lobbyInfo.RoomName} - Host IP: {lobbyInfo.RoomName}");

            SteamMatchmaking.LeaveLobby(lobbyid);
        }

        lobbies.Sort((a, b) => a.CreatedTime.CompareTo(b.CreatedTime));

        _completionSource.SetResult(lobbies);
    }

    public List<CSteamID> RequestLobbyMemberList()
    {
        List<CSteamID> memberList = new List<CSteamID>();

        CSteamID lobbyId = new CSteamID(CurrentLobbyId);

        int memberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyId);

        for(int i = 0; i < memberCount; i++)
        {
            CSteamID memberId = SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i);
            string memberName = SteamFriends.GetFriendPersonaName(memberId);

            Debug.Log($"Member {i}: {memberName}");

            memberList.Add(memberId);
        }

        return memberList;
    }

    public void StartGame()
    {
        string[] scenesToClose = new string[] { "" };
    }
}

public class LobbyInfo
{
    public CSteamID LobbyId;
    public string RoomName;
    public int MemberCount;
    public int MaxPlayers;
    public string OwnerName;
    public long CreatedTime;

    public void SetInfo(CSteamID lobbyid)
    {
        LobbyId = lobbyid;
        RoomName = SteamMatchmaking.GetLobbyData(lobbyid, "name");
        MemberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyid);
        MaxPlayers = SteamMatchmaking.GetLobbyMemberLimit(lobbyid);
        OwnerName = SteamFriends.GetFriendPersonaName(SteamMatchmaking.GetLobbyOwner(lobbyid));
        
        string timeStr = SteamMatchmaking.GetLobbyData(lobbyid, "created_time");
        long.TryParse(timeStr, out CreatedTime);
    }
}