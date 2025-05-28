using System.Collections.Generic;
using System.Threading.Tasks;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using Steamworks;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SteamManagerEx : IManager
{
    private TaskCompletionSource<List<LobbyInfo>> _completionSource;

    private Callback<LobbyCreated_t> _lobbyCreated;
    private Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
    private Callback<LobbyEnter_t> _lobbyEntered;
    private Callback<LobbyMatchList_t> _lobbyList;
    private Callback<LobbyDataUpdate_t> _lobbyDataUpdate;
    private Callback<LobbyChatUpdate_t> _lobbyChatUpdate;
    
    public ELobbyType LobbyType = ELobbyType.k_ELobbyTypePublic;

    public FishySteamworks.FishySteamworks FishySteamworks { get; private set; }

    public ulong CurrentLobbyId { get; private set; } = 0;
    public ulong SelectedLobbyId { get; set; } = 0;
    public LobbyInfo lobbyInfo = new LobbyInfo();

    private const string HostAddressKey = "HostAddress";

    private CSteamID _currentLobbyOwner;

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
            SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyId));
            SteamAPI.Shutdown();
    }

    private void RegisterCallbacks()
    {
        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        _lobbyList = Callback<LobbyMatchList_t>.Create(OnLobbyList);
        _lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        _lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
    }

    private void OnLobbyDataUpdate(LobbyDataUpdate_t callback)
    {
        // 현재 로비의 업데이트인지 확인
        if(callback.m_ulSteamIDLobby != CurrentLobbyId) return;

        CSteamID currentOwner = SteamMatchmaking.GetLobbyOwner(new CSteamID(CurrentLobbyId));
        
        // 호스트가 변경되었는지 확인
        if(_currentLobbyOwner.IsValid() && _currentLobbyOwner != currentOwner)
        {
            Debug.Log("Lobby owner has changed. Leaving lobby...");
            LeaveLobby();
            return;
        }

        _currentLobbyOwner = currentOwner;
    }

    private void OnLobbyChatUpdate(LobbyChatUpdate_t callback)
    {
        // 현재 로비의 업데이트인지 확인
        if(callback.m_ulSteamIDLobby != CurrentLobbyId) return;

        // 사용자가 로비를 떠났는지 확인
        if(callback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeLeft ||
           callback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeDisconnected ||
           callback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeKicked ||
           callback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeBanned)
        {
            CSteamID userWhoLeft = new CSteamID(callback.m_ulSteamIDUserChanged);
            
            // 떠난 사용자가 호스트인지 확인
            if(userWhoLeft == _currentLobbyOwner)
            {
                Debug.Log("Host has left the lobby. All users will be disconnected.");
                LeaveLobby();
            }
        }
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

        // 현재 로비 오너 저장
        _currentLobbyOwner = SteamMatchmaking.GetLobbyOwner(new CSteamID(CurrentLobbyId));
        Debug.Log($"Entered lobby. Current owner: {SteamFriends.GetFriendPersonaName(_currentLobbyOwner)}");

        FishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyId), HostAddressKey));
        FishySteamworks.StartConnection(false);
    }

    public bool JoinByID(ulong steamID)
    {
        Debug.Log("Attempting to join lobby with ID: " + steamID);

        if(SteamMatchmaking.RequestLobbyData(new CSteamID(steamID)))
        {
            // 로비 멤버 수 확인
            int memberCount = SteamMatchmaking.GetNumLobbyMembers(new CSteamID(steamID));
            
            if(memberCount == 0)
            {
                Debug.LogError("Failed to join lobby: Lobby is empty (no members)");
                return false;
            }
            
            Debug.Log($"Lobby has {memberCount} member(s). Attempting to join...");
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
        _currentLobbyOwner = CSteamID.Nil;

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

            //SteamMatchmaking.LeaveLobby(lobbyid);
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

    /// <summary>
    /// 게임 시작 후 로비를 다른 플레이어들에게 보이지 않게 설정합니다.
    /// </summary>
    public void SetLobbyInvisible()
    {
        if(CurrentLobbyId != 0)
        {
            // 로비 타입을 Private으로 변경하여 다른 플레이어들이 찾을 수 없게 합니다
            SteamMatchmaking.SetLobbyType(new CSteamID(CurrentLobbyId), ELobbyType.k_ELobbyTypePrivate);
            Debug.Log("Lobby set to invisible (private) after game start");
        }
    }

    /// <summary>
    /// 게임 종료 후 로비를 다시 보이게 설정합니다.
    /// </summary>
    public void SetLobbyVisible()
    {
        if(CurrentLobbyId != 0)
        {
            // 로비 타입을 다시 Public으로 변경하여 다른 플레이어들이 찾을 수 있게 합니다
            SteamMatchmaking.SetLobbyType(new CSteamID(CurrentLobbyId), LobbyType);
            Debug.Log("Lobby set to visible (public) after game end");
        }
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