using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Steamworks;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkRoomSystem : NetworkSingleton<NetworkRoomSystem>
{
    #region Constants
    private const float CONNECTION_TIMEOUT = 10f;
    private const float WAIT_INTERVAL = 0.1f;
    private const float INITIAL_DELAY = 0.1f;
    #endregion

    #region Fields
    public readonly SyncDictionary<NetworkConnection, ColorType> PlayerColors = new SyncDictionary<NetworkConnection, ColorType>();
    public readonly SyncDictionary<NetworkConnection, ulong> NetworkConnectionToSteamId = new SyncDictionary<NetworkConnection, ulong>();
    
    [SerializeField] private LobbyRoomUI _lobbyRoomUIPrefab;
    public LobbyRoomUI _lobbyRoomUI;
    
    private static ColorType[] AllColors => (ColorType[])Enum.GetValues(typeof(ColorType));
    #endregion

    #region Events
    public static event Action<ColorType[]> OnAvailableColorsReceived;
    #endregion

    #region Network Lifecycle
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        InstanceFinder.SceneManager.OnLoadEnd += OnLoadEnd;
        
        // Host인 경우 즉시 자신을 등록
        if (IsServerStarted)
        {
            StartCoroutine(InitializeHostDelayed());
        }
        else
        {
            StartCoroutine(InitializeClientDelayed());
        }
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        InstanceFinder.SceneManager.OnLoadEnd -= OnLoadEnd;
        CleanupClient();
    }

    private void OnLoadEnd(SceneLoadEndEventArgs args)
    {
        foreach(UnityEngine.SceneManagement.Scene scene in args.LoadedScenes)
        {
            if(scene.name == "Title")
            {
                CreateRoomUI();
            }
        }
    }

    private void CleanupClient()
    {
        RemovePlayer();
        HandlePlayerLeave(InstanceFinder.ClientManager.Connection);
        DestroyRoomUI(InstanceFinder.ClientManager.Connection);
    }
    #endregion

    #region Client Initialization
    private IEnumerator InitializeHostDelayed()
    {
        Debug.Log("NetworkRoomSystem: Host initialization started");
        
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(INITIAL_DELAY);
        
        if (!IsValidObject())
        {
            Debug.LogWarning("NetworkRoomSystem: Host object is invalid or inactive");
            yield break;
        }

        // Host 자신을 플레이어로 등록
        yield return StartCoroutine(RegisterHostPlayer());
        
        // UI 생성
        CreateRoomUI();
        
        Debug.Log("NetworkRoomSystem: Host initialization completed");
    }

    private IEnumerator RegisterHostPlayer()
    {
        if (Managers.Network.Type != NetworkType.Steam)
            yield break;

        // Steam 초기화 대기
        yield return new WaitUntil(() => SteamAPI.IsSteamRunning());
        
        ulong steamId = SteamUser.GetSteamID().m_SteamID;
        if (steamId == 0)
        {
            Debug.LogWarning("NetworkRoomSystem: Host has invalid Steam ID");
            yield break;
        }

        var hostConnection = InstanceFinder.ClientManager?.Connection;
        if (hostConnection == null)
        {
            Debug.LogWarning("NetworkRoomSystem: Host connection is null");
            yield break;
        }

        Debug.Log($"NetworkRoomSystem: Registering Host with Steam ID: {steamId}");
        
        // Host 데이터 직접 등록 (ServerRpc 호출하지 않음)
        if (NetworkConnectionToSteamId.ContainsKey(hostConnection))
            NetworkConnectionToSteamId[hostConnection] = steamId;
        else
            NetworkConnectionToSteamId.Add(hostConnection, steamId);

        // Host 색상 할당
        ColorType assignedColor = GetAvailableRandomColor();
        if (PlayerColors.ContainsKey(hostConnection))
            PlayerColors[hostConnection] = assignedColor;
        else
            PlayerColors.Add(hostConnection, assignedColor);
        
        Debug.Log($"NetworkRoomSystem: Host assigned color: {assignedColor}");
        
        // UI 업데이트 약간 지연
        yield return new WaitForSeconds(0.1f);
        UpdateUI();
    }

    private IEnumerator InitializeClientDelayed()
    {
        Debug.Log("NetworkRoomSystem: Client initialization started");
        
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(INITIAL_DELAY);
        
        if (!IsValidObject())
        {
            Debug.LogWarning("NetworkRoomSystem: Object is invalid or inactive");
            yield break;
        }

        yield return StartCoroutine(WaitForClientReady());
    }

    private IEnumerator WaitForClientReady()
    {
        var clientConnection = GetClientConnection();
        if (clientConnection == null)
        {
            Debug.LogWarning("NetworkRoomSystem: Client connection is null");
            yield break;
        }

        // 연결 활성화 대기
        yield return StartCoroutine(WaitForConnectionActive(clientConnection));
        clientConnection = GetClientConnection();
        if (clientConnection == null || !clientConnection.IsActive)
            yield break;

        // 클라이언트 시작 대기
        yield return StartCoroutine(WaitForClientStarted());
        if (!base.IsClientStarted)
            yield break;

        // // Observer 등록 대기
        // yield return StartCoroutine(WaitForObserverRegistration(clientConnection));
        // clientConnection = GetClientConnection();
        // if (clientConnection == null || !base.Observers.Contains(clientConnection))
        //     yield break;

        // Steam 플레이어 등록
        yield return StartCoroutine(RegisterSteamPlayer());

        // 방 입장 완료
        CompleteRoomJoin();
    }

    private IEnumerator WaitForConnectionActive(NetworkConnection connection)
    {
        Debug.Log("NetworkRoomSystem: Waiting for connection to activate...");
        
        yield return StartCoroutine(WaitForCondition(
            () => {
                connection = GetClientConnection();
                return connection != null && connection.IsActive;
            },
            "Connection activation",
            CONNECTION_TIMEOUT
        ));

        connection = GetClientConnection();
        bool success = connection != null && connection.IsActive;
        Debug.Log(success ? "NetworkRoomSystem: Connection activated successfully" : 
                          "NetworkRoomSystem: Connection activation failed");
    }

    private IEnumerator WaitForClientStarted()
    {
        Debug.Log("NetworkRoomSystem: Waiting for client to start...");
        
        yield return StartCoroutine(WaitForCondition(
            () => base.IsClientStarted && IsValidObject(),
            "Client startup",
            CONNECTION_TIMEOUT
        ));

        bool success = base.IsClientStarted && IsValidObject();
        Debug.Log(success ? "NetworkRoomSystem: Client started successfully" : 
                          "NetworkRoomSystem: Client startup failed");
    }

    private IEnumerator WaitForObserverRegistration(NetworkConnection connection)
    {
        Debug.Log("NetworkRoomSystem: Waiting for observer registration...");
        
        yield return StartCoroutine(WaitForCondition(
            () => {
                connection = GetClientConnection();
                return connection != null && connection.IsValid && 
                       base.Observers.Contains(connection) && IsValidObject();
            },
            "Observer registration",
            CONNECTION_TIMEOUT
        ));

        connection = GetClientConnection();
        bool success = connection != null && connection.IsValid && base.Observers.Contains(connection);
        Debug.Log(success ? "NetworkRoomSystem: Observer registration successful" : 
                          "NetworkRoomSystem: Observer registration failed");
    }

    private IEnumerator RegisterSteamPlayer()
    {
        if (Managers.Network.Type != NetworkType.Steam)
            yield break;

        ulong steamId = SteamUser.GetSteamID().m_SteamID;
        if (steamId == 0)
        {
            Debug.LogWarning("NetworkRoomSystem: Invalid Steam ID");
            yield break;
        }

        Debug.Log($"NetworkRoomSystem: Adding player with Steam ID: {steamId}");
        AddPlayer(steamId);
    }

    private void CompleteRoomJoin()
    {
        Debug.Log("NetworkRoomSystem: Completing room join...");
        HandlePlayerJoin();
        CreateRoomUI();
        Debug.Log("NetworkRoomSystem: Room join completed successfully");
    }
    #endregion

    #region Utility Methods
    private IEnumerator WaitForCondition(Func<bool> condition, string description, float timeout)
    {
        float elapsed = 0f;
        
        while (!condition() && elapsed < timeout)
        {
            Debug.Log($"NetworkRoomSystem: {description} waiting... ({elapsed:F1}s)");
            yield return new WaitForSeconds(WAIT_INTERVAL);
            elapsed += WAIT_INTERVAL;
        }
    }

    private NetworkConnection GetClientConnection() => InstanceFinder.ClientManager?.Connection;

    private bool IsValidObject() => this != null && gameObject != null && gameObject.activeInHierarchy;

    private bool IsValidConnection(NetworkConnection connection) =>
        connection != null && connection.IsValid && connection.IsActive;
    #endregion

    #region Player Management - Server RPCs
    [ServerRpc(RequireOwnership = false)]
    public void AddPlayer(ulong steamId, NetworkConnection connection = null)
    {
        if (!IsValidConnection(connection))
        {
            Debug.LogWarning($"NetworkRoomSystem.AddPlayer: Invalid connection - SteamID: {steamId}");
            return;
        }

        if (NetworkConnectionToSteamId.ContainsKey(connection))
        {
            Debug.LogWarning($"NetworkRoomSystem.AddPlayer: Connection {connection.ClientId} already exists - updating");
            NetworkConnectionToSteamId[connection] = steamId;
        }
        else
        {
            NetworkConnectionToSteamId.Add(connection, steamId);
            Debug.Log($"NetworkRoomSystem.AddPlayer: Added player - ClientID: {connection.ClientId}, SteamID: {steamId}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemovePlayer(NetworkConnection connection = null)
    {
        if (connection == null)
        {
            Debug.LogWarning("NetworkRoomSystem.RemovePlayer: Connection is null");
            return;
        }

        if (NetworkConnectionToSteamId.TryGetValue(connection, out ulong steamId))
        {
            NetworkConnectionToSteamId.Remove(connection);
            Debug.Log($"NetworkRoomSystem.RemovePlayer: Removed player - ClientID: {connection.ClientId}, SteamID: {steamId}");
        }
        else
        {
            Debug.LogWarning($"NetworkRoomSystem.RemovePlayer: Connection {connection.ClientId} not found");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandlePlayerJoin(NetworkConnection conn = null)
    {
        if (!IsValidConnection(conn))
        {
            Debug.LogWarning("NetworkRoomSystem.HandlePlayerJoin: Invalid connection");
            return;
        }

        ColorType assignedColor = GetAvailableRandomColor(conn);
        
        if (PlayerColors.ContainsKey(conn))
            PlayerColors[conn] = assignedColor;
        else
            PlayerColors.Add(conn, assignedColor);
        
        Debug.Log($"Player {conn.ClientId} assigned color: {assignedColor}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestJoinRoom(NetworkConnection conn = null) => HandlePlayerJoin(conn);

    [ServerRpc(RequireOwnership = false)]
    public void RequestLeaveRoom(NetworkConnection conn = null) => HandlePlayerLeave(conn);

    private void HandlePlayerLeave(NetworkConnection conn)
    {
        if (PlayerColors.TryGetValue(conn, out ColorType releasedColor))
        {
            PlayerColors.Remove(conn);
            Debug.Log($"Player {conn.ClientId} left and released color: {releasedColor}");
        }
    }
    #endregion

    #region Color Management
    [ServerRpc(RequireOwnership = false)]
    public void ChangeColor(ColorType colorType, NetworkConnection connection = null)
    {
        if (!IsColorAvailable(colorType, connection))
        {
            Debug.LogWarning($"Color {colorType} is already in use!");
            return;
        }

        if (PlayerColors.ContainsKey(connection))
        {
            PlayerColors[connection] = colorType;
            UpdateUI();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestAvailableColors(NetworkConnection conn = null)
    {
        List<ColorType> availableColors = GetAvailableColors(conn);
        TargetSendAvailableColors(conn, availableColors.ToArray());
    }

    [TargetRpc]
    private void TargetSendAvailableColors(NetworkConnection conn, ColorType[] availableColors)
    {
        OnAvailableColorsReceived?.Invoke(availableColors);
    }

    private ColorType GetAvailableRandomColor(NetworkConnection excludeConnection = null)
    {
        var usedColors = GetUsedColors(excludeConnection);
        var availableColors = AllColors.Where(color => !usedColors.Contains(color)).ToArray();

        if (availableColors.Length == 0)
        {
            Debug.LogWarning("모든 색상이 사용 중입니다! 첫 번째 색상을 할당합니다.");
            return AllColors[0];
        }

        return availableColors[Random.Range(0, availableColors.Length)];
    }

    private bool IsColorAvailable(ColorType color, NetworkConnection excludeConnection = null)
    {
        var usedColors = GetUsedColors(excludeConnection);
        return !usedColors.Contains(color);
    }

    public List<ColorType> GetAvailableColors(NetworkConnection excludeConnection = null)
    {
        var usedColors = GetUsedColors(excludeConnection);
        return AllColors.Where(color => !usedColors.Contains(color)).ToList();
    }

    private HashSet<ColorType> GetUsedColors(NetworkConnection excludeConnection = null)
    {
        return PlayerColors
            .Where(kvp => kvp.Key != excludeConnection)
            .Select(kvp => kvp.Value)
            .ToHashSet();
    }

    public Color GetPlayerColor(NetworkConnection connection)
    {
        if (PlayerColors.TryGetValue(connection, out ColorType colorType))
            return ColorSO.GetColor(colorType);
        return Color.white;
    }
    #endregion

    #region Steam Integration
    public ulong GetSteamId(NetworkConnection connection)
    {
        return NetworkConnectionToSteamId.TryGetValue(connection, out ulong steamId) ? steamId : 0;
    }
    #endregion

    #region UI Management
    private void CreateRoomUI()
    {
        Debug.Log("NetworkRoomSystem: CreateRoomUI called");
        
        if (_lobbyRoomUIPrefab == null)
        {
            Debug.LogError("NetworkRoomSystem: _lobbyRoomUIPrefab is null!");
            return;
        }

        if (_lobbyRoomUI != null)
        {
            Debug.Log("NetworkRoomSystem: Destroying existing lobby UI");
            Destroy(_lobbyRoomUI.gameObject);
        }

        _lobbyRoomUI = Instantiate(_lobbyRoomUIPrefab);
        Debug.Log($"NetworkRoomSystem: Created lobby UI: {_lobbyRoomUI != null}");
        
        // UI 생성 후 약간의 지연을 두고 업데이트
        if (IsServerStarted)
        {
            StartCoroutine(DelayedUIUpdate());
        }
    }

    private IEnumerator DelayedUIUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        if (_lobbyRoomUI != null)
        {
            Debug.Log("NetworkRoomSystem: Performing delayed UI update");
            UpdateUI();
        }
    }

    [TargetRpc]
    private void DestroyRoomUI(NetworkConnection conn)
    {
        if (_lobbyRoomUI != null)
            Destroy(_lobbyRoomUI.gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateUI()
    {
        Debug.Log("NetworkRoomSystem: UpdateUI called");
        ResetUI();

        if (Managers.Network.Type != NetworkType.Steam) 
        {
            Debug.Log("NetworkRoomSystem: Not Steam network type, skipping UI update");
            return;
        }

        int playerCount = 0;
        foreach (var connection in InstanceFinder.ServerManager.Clients)
        {
            ulong steamId = GetSteamId(connection.Value);
            if (steamId == 0) 
            {
                Debug.LogWarning($"NetworkRoomSystem: No Steam ID for connection {connection.Value.ClientId}");
                continue;
            }

            string name = SteamFriends.GetFriendPersonaName(new CSteamID(steamId)); 
            Color color = GetPlayerColor(connection.Value);
            
            Debug.Log($"NetworkRoomSystem: Creating UI for player - Name: {name}, Color: {color}");
            CreateUI(name, color);
            playerCount++;
        }
        
        Debug.Log($"NetworkRoomSystem: UpdateUI completed - {playerCount} players processed");
    }

    [ObserversRpc]
    private void ResetUI()
    {
        _lobbyRoomUI?.ResetUI();
    }

    [ObserversRpc]
    private void CreateUI(string name, Color color)
    {
        _lobbyRoomUI?.CreateUI(name, color);
    }
    #endregion

    public string GetPlayerName(NetworkConnection connection)
    {
        ulong steamId = GetSteamId(connection);
        if (steamId == 0) 
            return ""; 

        string name = SteamFriends.GetFriendPersonaName(new CSteamID(steamId));
        return name;
    }


}

