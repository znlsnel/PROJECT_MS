using FishNet;
using FishNet.Managing;
using Steamworks;
using UnityEngine;

public class SteamManagerEx : MonoBehaviour
{
    private static SteamManagerEx instance;
    public static SteamManagerEx Instance => instance;

    [field: SerializeField] public int maxConnections { get; set; } = 10;
    [field: SerializeField] public string networkAddress { get; set; } = "127.0.0.1";

    [field: SerializeField] public FishySteamworks.FishySteamworks FishySteamworks { get; private set; }
    
    protected Callback<LobbyCreated_t> _lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> _lobbyEntered;
    protected Callback<LobbyMatchList_t> _lobbyList;
    
    public ELobbyType LobbyType = ELobbyType.k_ELobbyTypePublic;

    public static ulong CurrentLobbyId { get; private set; } = 0;

    private const string HostAddressKey = "HostAddress";

    public void Awake()
    {
        instance = this;
        FishySteamworks = GetComponent<FishySteamworks.FishySteamworks>();

        SteamAPI.Init();
    }

    public void Start()
    {
        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        _lobbyList = Callback<LobbyMatchList_t>.Create(OnLobbyList);
    }

    public void Update()
    {
        SteamAPI.RunCallbacks();
    }

    public void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(LobbyType, maxConnections);
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

    public static void JoinByID(CSteamID steamID)
    {
        Debug.Log("Attempting to join lobby with ID: " + steamID.m_SteamID);

        if(SteamMatchmaking.RequestLobbyData(steamID))
            SteamMatchmaking.JoinLobby(steamID);
        else
            Debug.LogError("Failed to join lobby with ID: " + steamID.m_SteamID);

    }

    public static void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyId));
        CurrentLobbyId = 0;

        instance.FishySteamworks.StopConnection(false);
        if(InstanceFinder.NetworkManager.IsServerStarted)
            instance.FishySteamworks.StopConnection(true);

    }

    private void OnLobbyList(LobbyMatchList_t callback)
    {
        
    }

    public void StartGame()
    {
        string[] scenesToClose = new string[] { "" };
    }
}
